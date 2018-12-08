using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Share.Models.Article.Entities;

namespace DbContexts.Article
{
    public class ArticleCommentConfig : IEntityTypeConfiguration<ArticleComment>
    {
        public void Configure(EntityTypeBuilder<ArticleComment> e)
        {
            e.ToTable("ArticleComment");
            e.Property(p => p.ArticleCommentId).ValueGeneratedNever();
            e.Property(p => p.ArticleId).IsRequired();
            e.Property(p => p.Comment).IsRequired().IsUnicode(false);
            e.Property(p => p.UserIdOrEmail).IsRequired().IsUnicode(false).HasMaxLength(100);
            e.Property(p => p.UserSocialAvatarUrl).IsUnicode(false);
            e.Property(p => p.IsAdminCommented);
            e.Property(p => p.CommentedDate).IsRequired().HasColumnType("datetime2(7)");
        }
    }
}