using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Share.Models.Article.Dtos
{
    public class ArticleCommitDto
    {
        public long ArticleCommitId { get; set; }
        public string Commit { get; set; }
        public DateTime? CommittedDate { get; set; }
        public string UserIdOrEmail { get; set; }
        public string UserSocialAvatarUrl { get; set; }
        public bool? IsAdminCommited { get; set; }   // typo mistakes can fix by any one, courtesy
        public long ArticleId { get; set; }
    }
}
