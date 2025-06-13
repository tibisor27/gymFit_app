using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}