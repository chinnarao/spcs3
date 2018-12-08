using Microsoft.EntityFrameworkCore;

namespace DbContexts.Article
{
    public class ArticleDbContext : DbContext
    {
        public ArticleDbContext(DbContextOptions<ArticleDbContext> options) : base(options){}

        public DbSet<Share.Models.Article.Entities.Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ArticleConfig());
            modelBuilder.ApplyConfiguration(new ArticleLicenseConfig());
            modelBuilder.ApplyConfiguration(new ArticleCommentConfig());
            modelBuilder.ApplyConfiguration(new ArticleCommitConfig());
        }
    }
}
