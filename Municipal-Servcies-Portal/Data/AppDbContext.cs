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
        public DbSet<Issue> Issues { get; set; }

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
            
            // Configure Issue entity
            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Location).IsRequired().HasMaxLength(200);
                entity.Property(i => i.Category).IsRequired().HasMaxLength(100);
                entity.Property(i => i.Description).IsRequired().HasMaxLength(2000);
                entity.Property(i => i.Status).HasMaxLength(50).HasDefaultValue("Pending");
                entity.Property(i => i.AttachmentPathsJson).HasMaxLength(4000);
                entity.Property(i => i.NotificationEmail).HasMaxLength(200);
                entity.Property(i => i.NotificationPhone).HasMaxLength(20);
                entity.Property(i => i.AssignedTo).HasMaxLength(100);
                entity.Property(i => i.IsActive).HasDefaultValue(true);
                
                // Index for common queries
                entity.HasIndex(i => i.DateReported);
                entity.HasIndex(i => i.Status);
                entity.HasIndex(i => i.Category);
            });
        }
    }
}
