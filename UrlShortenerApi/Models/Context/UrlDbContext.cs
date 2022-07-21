using Microsoft.EntityFrameworkCore;

namespace UrlShortenerApi.Models.Context
{
    public class UrlDbContext : DbContext
    {
        public DbSet<Url> Urls { get; set; }

        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>().ToTable("Urls");
        }
    }
}
