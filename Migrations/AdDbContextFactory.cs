using DbContexts.Ad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Migrations
{
    public class AdDbContextFactory : IDesignTimeDbContextFactory<AdDbContext>
    {
        public AdDbContext CreateDbContext(string[] args) //you can ignore args, maybe on later versions of .net core it will be used but right now it isn't
        {
            string connection = "Server=localhost;Database=Ad;Trusted_Connection=True;";
            var optionBuilder = new DbContextOptionsBuilder<AdDbContext>();
            //var options = optionBuilder.UseSqlServer(connection, a => a.MigrationsAssembly("Migrations").UseNetTopologySuite());
            var options = optionBuilder.UseSqlServer(connection, a => a.MigrationsAssembly($"{nameof(Migrations)}").UseNetTopologySuite());
            return new AdDbContext(optionBuilder.Options);
        }
    }
}
