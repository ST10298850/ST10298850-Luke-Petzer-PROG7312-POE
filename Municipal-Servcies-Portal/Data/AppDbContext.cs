using Microsoft.EntityFrameworkCore;
using Municipal_Servcies_Portal.Models;

namespace Municipal_Servcies_Portal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            // Configure Event entity
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.ImagePath).HasMaxLength(500);
            });

            // Configure Announcement entity
            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.HasKey(a => a.Id);
            });
        }
    }
}

