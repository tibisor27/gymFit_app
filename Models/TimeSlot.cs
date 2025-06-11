using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.Models
{
    public class TimeSlot
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        public DateTime EndTime { get; set; }

        public bool IsBooked { get; set; } = false;
    }
} 