using Microsoft.EntityFrameworkCore;
using VideoGameApi.Models;

namespace VideoGameApi.Data
{
    public class VideoGameDbContext(DbContextOptions<VideoGameDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoGame>()
                .HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Character>()
                .HasQueryFilter(c => !c.IsDeleted);
        }
        public DbSet<VideoGame> VideoGames { get; set; }
        public DbSet<Character> Characters { get; set; }

    }
    
}
