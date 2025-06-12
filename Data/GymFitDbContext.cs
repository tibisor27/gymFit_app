using Microsoft.EntityFrameworkCore;
using GymFit.BE.Models;

namespace GymFit.BE.Data
{
    public class GymFitDbContext : DbContext
    {
        public GymFitDbContext(DbContextOptions<GymFitDbContext> options) : base(options)
        {
        }

        // DbSets pentru toate entitățile
        public DbSet<User> Users { get; set; }
        public DbSet<Members> Members { get; set; }
        public DbSet<Trainers> Trainers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<GymClass> GymClasses { get; set; }

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

            // Configure Session entity
            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // STRUCTURĂ pentru money
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                
                // ENUM conversion
                entity.Property(e => e.Status).HasConversion<int>();

                // RELAȚII
                entity.HasOne(e => e.Member)
                      .WithMany()
                      .HasForeignKey(e => e.MemberId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Trainer)
                      .WithMany(t => t.Sessions)
                      .HasForeignKey(e => e.TrainerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.GymClass)
                      .WithMany()
                      .HasForeignKey(e => e.GymClassId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Membership entity
            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // STRUCTURĂ pentru money
                entity.Property(e => e.PaidAmount).HasColumnType("decimal(18,2)");
                
                // ENUM conversion
                entity.Property(e => e.Status).HasConversion<int>();

                // RELAȚII
                entity.HasOne(e => e.Member)
                      .WithMany()
                      .HasForeignKey(e => e.MemberId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.MembershipType)
                      .WithMany(mt => mt.Memberships)
                      .HasForeignKey(e => e.MembershipTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure MembershipType entity
            modelBuilder.Entity<MembershipType>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // STRUCTURĂ pentru money
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                
                // STRUCTURĂ text
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Configure GymClass entity - SIMPLU
            modelBuilder.Entity<GymClass>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Category).HasMaxLength(50);
            });
        }
    }
} 