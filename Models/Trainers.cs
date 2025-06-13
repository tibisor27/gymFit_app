using System.ComponentModel.DataAnnotations;
using GymFit.BE.Models;

namespace GymFit.BE.Models
{
    public class Trainer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User is required")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Required(ErrorMessage = "Experience is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Experience must be between 5 and 200 characters")]
        public required string Experience { get; set; }

        [Required(ErrorMessage = "Introduction is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Introduction must be between 10 and 500 characters")]
        public required string Introduction { get; set; }

        // SIMPLU - fără relații complexe!

    }
}