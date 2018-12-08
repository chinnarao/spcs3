using DbContexts.Article;
using Repository;
using Share.Models.Article.Entities;
using Share.Models.Article.Dtos;
using Share.Extensions;
using System;
using System.IO;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Services.Commmon;
using Share._3rdParty;

namespace Services.Article
{
    public class ArticleService : IArticleService
    {
        private readonly ILogger _logger;
        private readonly IFileRead _fileReadService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IGoogleStorage _googleStorage;
        private readonly IRepository<Share.Models.Article.Entities.Article, ArticleDbContext> _articleRepository;
        private readonly IRepository<ArticleCommit, ArticleDbContext> _articleCommitRepository;
        private readonly IRepository<ArticleComment, ArticleDbContext> _articleCommentRepository;
        private readonly IRepository<ArticleLicense, ArticleDbContext> _articleLicenseRepository;

        public ArticleService(ILogger<ArticleService> logger, IMapper mapper, ICacheService cacheService, IFileRead fileReadService, IGoogleStorage googleStorage,
                                IRepository<Share.Models.Article.Entities.Article, ArticleDbContext> articleRepository,
                                IRepository<ArticleCommit, ArticleDbContext> articleCommitRepository,
                                IRepository<ArticleComment, ArticleDbContext> articleCommentRepository,
                                IRepository<ArticleLicense, ArticleDbContext> articleLicenseRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _fileReadService = fileReadService;
            _cacheService = cacheService;
            _googleStorage = googleStorage;
            _articleRepository = articleRepository;
            _articleCommitRepository = articleCommitRepository;
            _articleCommentRepository = articleCommentRepository;
            _articleLicenseRepository = articleLicenseRepository;
        }

        #region CreateArticle
        public ArticleDto CreateArticle(ArticleDto dto)
        {
            // transaction has to implement or not , has to think more required.
            ArticleDto articleDto = this.InsertArticle(dto);
            dto.GoogleStorageArticleFileDto.ArticleAnonymousDataObjectForHtmlTemplate = GetArticleAsAnonymousObjectForHtmlTemplate(dto);
            this.UploadObjectInGoogleStorage(dto.GoogleStorageArticleFileDto);
            return articleDto;
        }
        private ArticleDto InsertArticle(ArticleDto dto)
        {
            Share.Models.Article.Entities.Article article = _mapper.Map<Share.Models.Article.Entities.Article>(dto);
            RepositoryResult result = _articleRepository.Create(article);
            if (!result.Succeeded) throw new Exception(string.Join(",", result.Errors));
            return dto;
        }
        private void UploadObjectInGoogleStorage(GoogleStorageArticleFileDto model)
        {
            if (model == null) throw new ArgumentNullException(nameof(GoogleStorageArticleFileDto));
            if (model.ArticleAnonymousDataObjectForHtmlTemplate == null) throw new ArgumentNullException(nameof(model.ArticleAnonymousDataObjectForHtmlTemplate));
            string content = _cacheService.Get<string>(model.CACHE_KEY);
            if (string.IsNullOrWhiteSpace(content))
            {
                content = System.IO.File.ReadAllText(model.HtmlFileTemplateFullPathWithExt);
                if (string.IsNullOrEmpty(content)) throw new Exception(nameof(content));
                content = _cacheService.GetOrAdd<string>(model.CACHE_KEY, () => content, model.CacheExpiryDateTimeForHtmlTemplate);
                if (string.IsNullOrEmpty(content)) throw new Exception(nameof(content));
            }
            content = "";// content.FillContent(model.ArticleAnonymousDataObjectForHtmlTemplate);
            if (string.IsNullOrEmpty(content)) throw new Exception(nameof(content));
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            if (stream == null || stream.Length <= 0) throw new Exception(nameof(stream));
            _googleStorage.UploadObject(model.GoogleStorageBucketName, stream, model.GoogleStorageObjectNameWithExt, model.ContentType);
        }
        private dynamic GetArticleAsAnonymousObjectForHtmlTemplate(ArticleDto dto)
        {
            return new
            {
                tag1 = dto.Tag1,
                tag2 = dto.Tag2,
                tag3 = dto.Tag3,
                tag4 = dto.Tag4,
                tag5 = dto.Tag5,
            };
        }
        #endregion

        //https://github.com/daniyalf/CampusTalk/blob/e90fd6286635fef440a244593b87e28cfef2a93f/CampusChat/Controllers/PostsController.cs
        public dynamic SearchArticles(ArticleSortFilterPageOptions options)
        {
            var articleDtos = _articleRepository.Entities.Where( w => w.IsPublished && w.IsActive).AsNoTracking()
                            .Select(s => new ArticleDto() { ArticleId = s.ArticleId,
                                Title = s.Title,
                                UpdatedDateTimeString = s.UpdatedDateTime.TimeAgo(),
                                UserIdOrEmail = s.UserIdOrEmail,
                                AllRelatedSubjectsIncludesVersionsWithComma = s.AllRelatedSubjectsIncludesVersionsWithComma })
                            .OrderByDescending(a => a.CreatedDateTime)
                            .OrderByDescending(a => a.UpdatedDateTime)
                            .Take(options.DefaultPageSize).ToList();
            options.SetupRestOfDto(articleDtos.Count);
            return new { articles = articleDtos, option = options };
        }

        public dynamic GetArticleLicenseCommentsCommits(long articleId)
        {

            var anonymous = _articleRepository.Entities.Where(a => a.ArticleId == articleId)
                            .Select(s => new { License = s.ArticleLicense.License,
                                ArticleCommits = s.ArticleCommits.OrderByDescending(x => x.CommittedDate).ToList(),
                                ArticleComments = s.ArticleComments.OrderByDescending(x => x.CommentedDate)
                            });
            return anonymous;
        }

        public ArticleDto GetArticleDetail(long articleId)
        {
            var article = _articleRepository.Entities.Include(i => i.ArticleLicense).Include(i => i.ArticleCommits).Include(i => i.ArticleComments).AsNoTracking().First(i => i.ArticleId == articleId);
            ArticleDto articleDto = _mapper.Map<ArticleDto>(article);
            return articleDto;
        }

        public ArticleCommentDto CreateComment(ArticleCommentDto articleCommentDto)
        {
            ArticleComment articleComment = _mapper.Map<ArticleComment>(articleCommentDto);
            _articleCommentRepository.Create(articleComment);
            return articleCommentDto;
        }

        public ArticleDto UpdateArticleWithCommitHistory(ArticleDto articleDto)
        {
            Share.Models.Article.Entities.Article articleExisting = _articleRepository.Entities.Include(a => a.ArticleLicense).Single(a => a.ArticleId == articleDto.ArticleId);
            if (!string.Equals(articleExisting?.ArticleLicense?.License, articleDto?.ArticleLicenseDto?.License))
                articleDto.ArticleLicenseDto.LicensedDate = DateTime.UtcNow;
            articleExisting = _mapper.Map<ArticleDto, Share.Models.Article.Entities.Article>(articleDto, articleExisting);
            int i = _articleRepository.SaveChanges();
            ArticleDto articleDtoNew = _mapper.Map<ArticleDto>(articleExisting);
            return articleDtoNew;
        }

        public HashSet<string> GetAllUniqueTags()
        {
            List<dynamic> list = _articleRepository.Entities.Select(a => new { a.Tag1, a.Tag2, a.Tag3, a.Tag4, a.Tag5, a.Tag6, a.Tag7, a.Tag8, a.Tag9, a.Tag10, a.Tag11, a.Tag12 }).ToList<dynamic>();
            HashSet<string> t = new HashSet<string>();
            foreach (var i in list)
            {
                t.Add(i.Tag1); t.Add(i.Tag2); t.Add(i.Tag3); t.Add(i.Tag4); t.Add(i.Tag5); t.Add(i.Tag6); t.Add(i.Tag7); t.Add(i.Tag8); t.Add(i.Tag9); t.Add(i.Tag10); t.Add(i.Tag11); t.Add(i.Tag12);
            }
            return t;
        }
    }

    public interface IArticleService
    {
        ArticleDto CreateArticle(ArticleDto dto);
        dynamic SearchArticles(ArticleSortFilterPageOptions options);
        dynamic GetArticleLicenseCommentsCommits(long articleId);
        ArticleDto GetArticleDetail(long articleId);
        ArticleCommentDto CreateComment(ArticleCommentDto articleCommentDto);
        ArticleDto UpdateArticleWithCommitHistory(ArticleDto articleDto);
        HashSet<string> GetAllUniqueTags();
    }
}
