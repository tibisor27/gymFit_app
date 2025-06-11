using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.Models
{
    public class Membership
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MembershipTypeId { get; set; }
        public required String MembershipType { get; set; }

        public required float Price { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public MembershipStatus Status { get; set; } = MembershipStatus.Active;

        // Business info
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }

        // Business validation
        public bool IsActive => Status == MembershipStatus.Active && DateTime.Now <= EndDate;
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