using System;
using System.ComponentModel.DataAnnotations;

namespace Share.Models.Article.Dtos
{
    public class ArticleCommentDto
    {
        public long ArticleCommentId { get; set; }
        public long ArticleId { get; set; }
        public string Comment { get; set; }
        public string UserIdOrEmail { get; set; }
        public string UserSocialAvatarUrl { get; set; }
        public bool? IsAdminCommented { get; set; }
        public DateTime CommentedDate { get; set; }
    }
}
