using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Share.Models.Article.Entities;

namespace DbContexts.Article
{
    public class ArticleLicenseConfig : IEntityTypeConfiguration<ArticleLicense>
    {
        public void Configure(EntityTypeBuilder<ArticleLicense> e)
        {
            e.ToTable("ArticleLicense");
            e.Property(p => p.ArticleLicenseId).ValueGeneratedNever();
            e.Property(p => p.ArticleId).IsRequired();
            e.Property(p => p.License).IsUnicode(false);
            e.Property(p => p.LicensedDate).HasColumnType("datetime2(7)");
        }
    }
}