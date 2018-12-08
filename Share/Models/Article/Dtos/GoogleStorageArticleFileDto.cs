using System;
namespace Share.Models.Article.Dtos
{
    public class GoogleStorageArticleFileDto
    {
        public DateTime CacheExpiryDateTimeForHtmlTemplate { get; set; }
        public string GoogleStorageObjectNameWithExt { get; set; }
        public string GoogleStorageBucketName { get; set; }
        public dynamic ArticleAnonymousDataObjectForHtmlTemplate { get; set; }
        public string ContentType { get; set; }
        public string CACHE_KEY { get; set; }
        public string HtmlFileTemplateFullPathWithExt { get; set; }
    }
}
