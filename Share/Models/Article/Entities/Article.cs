using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Share.Models.Article.Entities
{
    public class Article
    {
        public long ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }     // navachar, article content , which is a big information
        public string UserIdOrEmail { get; set; }
        public string UserLoggedInSocialProviderName { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserSocialAvatarUrl { get; set; }
        public string BiodataUrl { get; set; } // user can provide his resume or profile url
        public bool HireMe { get; set; }
        public string OpenSourceUrls { get; set; }  // if user has any github then he or she can show his info
        public bool IsActive { get; set; }
        public bool IsArticleInDraftMode { get; set; }
        public bool IsPublished { get; set; }
        public int AttachedAssetsInCloudCount { get; set; }
        public Guid AttachedAssetsInCloudStorageId { get; set; }
        public string AttachedAssetsStoredInCloudBaseFolderPath { get; set; }

        public string AllRelatedSubjectsIncludesVersionsWithComma { get; set; }   // ex: chapter,section,subsection in codeproject
        public string AttachmentURLsComma { get; set; }     // source code , git hub urls can provide 
        public DateTime? PublishedDate { get; set; }
        public string SocialURLsWithComma { get; set; }     // user facebook or twitter url can provide
        public int? TotalVotes { get; set; }                   // article how many votes got from readers
        public int? TotalVotedPersonsCount { get; set; }
        public double? ArticleAverageVotes { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }

        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
        public string Tag6 { get; set; }
        public string Tag7 { get; set; }
        public string Tag8 { get; set; }
        public string Tag9 { get; set; }
        public string Tag10 { get; set; }
        public string Tag11 { get; set; }
        public string Tag12 { get; set; }

        public ArticleLicense ArticleLicense { get; set; }
        public IList<ArticleComment> ArticleComments { get; set; } = new List<ArticleComment>();
        public IList<ArticleCommit> ArticleCommits { get; set; } = new List<ArticleCommit>();
    }
}
