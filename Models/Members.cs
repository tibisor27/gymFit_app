using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.Models
{
    public class Members
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public bool IsActive { get; set; } = true;
        public DateTime JoinedAt { get; set; } = DateTime.Now;
    }
}