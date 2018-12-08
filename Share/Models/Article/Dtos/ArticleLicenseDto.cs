using System;
using System.Collections.Generic;
using System.Text;

namespace Share.Models.Article.Dtos
{
    public class ArticleLicenseDto
    {
        public long ArticleLicenseId { get; set; }
        public string License { get; set; }     // every article owner may have special license, which should be big text
        public DateTime? LicensedDate { get; set; }
        //Foreign key for Article
        public long ArticleId { get; set; }
    }
}
