using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }

        // Cine participă
        [Required]
        public int MemberId { get; set; }
        public required Members Member { get; set; } 

        [Required]
        public int TrainerId { get; set; }
        public required Trainers Trainer { get; set; }

        // Ce tip de sesiune (opțional pentru personal training)
        public int? GymClassId { get; set; }
        public GymClass? GymClass { get; set; }

        // Când se întâmplă
        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        // Status management
        [Required]
        public SessionStatus Status { get; set; } = SessionStatus.Scheduled;

        // Business info
        public decimal Price { get; set; }
        public string? Notes { get; set; }
        
        // Tracking
        public DateTime BookedAt { get; set; } = DateTime.Now;
        public DateTime? CancelledAt { get; set; }

        // Business validation
        public bool IsUpcoming => Status == SessionStatus.Scheduled && StartDateTime > DateTime.Now;
        public bool CanBeCancelled => IsUpcoming && (StartDateTime - DateTime.Now).TotalHours >= 2;
        public TimeSpan Duration => EndDateTime - StartDateTime;
    }

    public enum SessionStatus
    {
        Scheduled = 0,     // Programată
        InProgress = 1,    // În desfășurare
        Completed = 2,     // Finalizată
        Cancelled = 3,     // Anulată de member
        NoShow = 4,        // Member nu s-a prezentat
        TrainerCancelled = 5 // Anulată de trainer
    }
} 