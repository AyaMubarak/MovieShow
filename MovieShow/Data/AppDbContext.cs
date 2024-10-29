using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieShow.Models;

namespace MovieShow.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Serie>()
                .HasMany(s => s.Season)
                .WithOne(s => s.Serie)
                .HasForeignKey(s => s.SerieId);

            modelBuilder.Entity<Season>()
                .HasMany(se => se.Episode)
                .WithOne(e => e.Season)
                .HasForeignKey(e => e.SeasonId);
        }
    }
}