using Microsoft.EntityFrameworkCore;
using GymFit.API.Models;

namespace GymFit.API.Data
{
    public class GymFitDbContext : DbContext
    {
        public GymFitDbContext(DbContextOptions<GymFitDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<GymClass> GymClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UserRole).IsRequired();
                entity.Property(e => e.UserRole).HasConversion<int>(); // Store enum as int
            });

            // Configure GymClass entity
            modelBuilder.Entity<GymClass>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                
                // Configure relationship
                entity.HasOne(e => e.Trainer)
                      .WithMany()
                      .HasForeignKey(e => e.TrainerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Name = "John Doe", 
                    Email = "john.doe@email.com", 
                    Password = "hashedpassword123", 
                    UserRole = Role.User,
                    PhoneNumber = "123456789",
                    DateOfBirth = new DateOnly(1990, 1, 15)
                },
                new User 
                { 
                    Id = 2, 
                    Name = "Jane Smith", 
                    Email = "jane.smith@email.com", 
                    Password = "hashedpassword456", 
                    UserRole = Role.Trainer,
                    PhoneNumber = "987654321",
                    DateOfBirth = new DateOnly(1985, 8, 22)
                },
                new User 
                { 
                    Id = 3, 
                    Name = "Admin User", 
                    Email = "admin@gymfit.com", 
                    Password = "hashedpassword789", 
                    UserRole = Role.Admin,
                    PhoneNumber = "555000111",
                    DateOfBirth = new DateOnly(1980, 12, 5)
                }
            );

            modelBuilder.Entity<GymClass>().HasData(
                new GymClass { Id = 1, Name = "Morning Yoga", Description = "Relaxing yoga session", TrainerId = 2, MaxCapacity = 15, Duration = 60, Price = 25.00m },
                new GymClass { Id = 2, Name = "HIIT Training", Description = "High intensity workout", TrainerId = 2, MaxCapacity = 10, Duration = 45, Price = 35.00m }
            );
        }
    }
} 