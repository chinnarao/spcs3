using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Share.Models.Article.Entities
{
    public class ArticleComment
    {
        public long ArticleCommentId { get; set; }
        public long ArticleId { get; set; }
        public string UserIdOrEmail { get; set; }
        public string UserSocialAvatarUrl { get; set; }
        public bool? IsAdminCommented { get; set; }
        public DateTime CommentedDate { get; set; }
        public string Comment { get; set; }
        public Article Article { get; set; }
    }
}
