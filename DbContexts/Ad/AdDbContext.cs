using Microsoft.EntityFrameworkCore;

namespace DbContexts.Ad
{
    public class AdDbContext : DbContext
    {
        public AdDbContext(DbContextOptions<AdDbContext> options) : base(options) {}

        public DbSet<Share.Models.Ad.Entities.Ad> Ads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdConfig());
        }
    }
}
