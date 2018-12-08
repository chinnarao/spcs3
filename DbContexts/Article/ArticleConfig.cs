using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbContexts.Article
{
    public class ArticleConfig : IEntityTypeConfiguration<Share.Models.Article.Entities.Article>
    {
        public void Configure(EntityTypeBuilder<Share.Models.Article.Entities.Article> e)
        {
            e.ToTable("Article");
            e.Property(p => p.ArticleId).ValueGeneratedNever();
            e.Property(p => p.ArticleAverageVotes);
            e.Property(p => p.AllRelatedSubjectsIncludesVersionsWithComma).IsUnicode(false).HasMaxLength(1000);
            e.Property(p => p.AttachedAssetsInCloudCount);
            e.Property(p => p.AttachedAssetsInCloudStorageId).IsRequired();
            e.Property(p => p.AttachedAssetsStoredInCloudBaseFolderPath).IsUnicode(false);
            e.Property(p => p.AttachmentURLsComma).IsUnicode(false);
            e.Property(x => x.BiodataUrl).IsUnicode(false);
            e.Property(x => x.Content).IsRequired();
            e.Property(x => x.CreatedDateTime).IsRequired().HasColumnType("datetime2(7)");
            e.Property(x => x.HireMe).HasDefaultValue<bool>(false);
            e.Property(x => x.IsActive).HasDefaultValue<bool>(false);
            e.Property(x => x.IsArticleInDraftMode).HasDefaultValue<bool>(false);
            e.Property(x => x.IsPublished).HasDefaultValue<bool>(false);
            e.Property(x => x.OpenSourceUrls).IsUnicode(false);
            e.Property(x => x.PublishedDate).HasColumnType("datetime2(7)");

            e.Property(x => x.SocialURLsWithComma).IsUnicode(false);
            e.Property(x => x.Tag1).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag2).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag3).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag4).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag5).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag6).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag7).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag8).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag9).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag10).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag11).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Tag12).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.Title).IsRequired().IsUnicode(false);
            e.Property(x => x.TotalVotedPersonsCount);
            e.Property(x => x.TotalVotes);
            e.Property(x => x.UpdatedDateTime).IsRequired().HasColumnType("datetime2(7)");
            e.Property(x => x.UserIdOrEmail).IsRequired().IsUnicode(false).HasMaxLength(100);
            e.Property(x => x.UserLoggedInSocialProviderName).IsUnicode(false).HasMaxLength(32);
            e.Property(x => x.UserPhoneNumber).IsUnicode(false).HasMaxLength(15);
            e.Property(x => x.UserSocialAvatarUrl).IsUnicode(false);
        }
    }
}