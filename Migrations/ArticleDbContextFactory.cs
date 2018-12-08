using DbContexts.Article;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Migrations
{
    public class ArticleDbContextFactory : IDesignTimeDbContextFactory<ArticleDbContext>
    {
        public ArticleDbContext CreateDbContext(string[] args) //you can ignore args, maybe on later versions of .net core it will be used but right now it isn't
        {
            string connection = "Server=localhost;Database=Article;Trusted_Connection=True;";
            var optionBuilder = new DbContextOptionsBuilder<ArticleDbContext>();
            var options = optionBuilder.UseSqlServer(connection, a => a.MigrationsAssembly("Migrations"));
            return new ArticleDbContext(optionBuilder.Options);
        }
    }
}
