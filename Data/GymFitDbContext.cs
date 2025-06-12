using Microsoft.EntityFrameworkCore;
using GymFit.BE.Models;

namespace GymFit.BE.Data
{
    public class GymFitDbContext : DbContext
    {
        public GymFitDbContext(DbContextOptions<GymFitDbContext> options) : base(options)
        {
        }

        // DbSets pentru modelele esențiale - SIMPLU!
        public DbSet<User> Users { get; set; }
        public DbSet<Members> Members { get; set; }
        public DbSet<Trainers> Trainers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity - DOAR STRUCTURĂ
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // STRUCTURĂ DB - nu business validations
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Password).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(100);
                
                // INDEX pentru performance
                entity.HasIndex(e => e.Email).IsUnique();
                
                // ENUM conversion
                entity.Property(e => e.UserRole).HasConversion<int>();
            });

            // Configure Members entity
            modelBuilder.Entity<Members>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Trainers entity
            modelBuilder.Entity<Trainers>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Experience).HasMaxLength(200);
                entity.Property(e => e.Introduction).HasMaxLength(500);
                
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
} 