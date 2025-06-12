using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.Models
{
    public class Membership
    {
        [Key]
        public int Id { get; set; }

        // FK către member - memberul care deține abonamentul
        [Required]
        public int MemberId { get; set; }
        public required Members Member { get; set; }

        [Required]
        public int MembershipTypeId { get; set; }
        public required MembershipType MembershipType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public MembershipStatus Status { get; set; } = MembershipStatus.Active;
        
        [Required]
        public decimal PaidAmount { get; set; }
        public DateTime? PaymentDate { get; set; }

        // Tracking
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }

        // Business validation
        public bool IsActive => Status == MembershipStatus.Active && DateTime.Now <= EndDate;
        public int DaysRemaining => (EndDate - DateTime.Now).Days;
    }

    public class MembershipType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; } // "Basic", "Premium", "VIP"

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int DurationDays { get; set; } // 30, 90, 365

        [Required]
        public int IncludedSessions { get; set; } // Câte sesiuni include

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    }

    public enum MembershipStatus
    {
        Active = 0,
        Expired = 1,
        Cancelled = 2,
        Suspended = 3,
        PendingPayment = 4
    }
} 