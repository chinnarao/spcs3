using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Share.Models.Article.Entities
{
    public class ArticleLicense
    {
        public long ArticleLicenseId { get; set; }
        public long ArticleId { get; set; }
        public string License { get; set; }     // every article owner may have special license, which should be big text
        public DateTime? LicensedDate { get; set; }
        public Article Article { get; set; }
    }
}
