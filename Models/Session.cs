using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }
        public required User Member { get; set; } // User cu Role = User

        [Required]
        public int TrainerId { get; set; }
        public required Trainers Trainer { get; set; }

        [Required]
        public SessionStatus Status { get; set; } = SessionStatus.Scheduled;

        public int TimeSlotId { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public decimal Price { get; set; }

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