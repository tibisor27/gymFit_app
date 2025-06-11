using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.Models
{
    // Tabel de legătură (Junction Table) pentru Many-to-Many
    public class GymClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; } // "Yoga", "HIIT", "Personal Training"

        [StringLength(500)]
        public string? Description { get; set; } // Descriere detaliată

        [Required]
        public required string Category { get; set; } // "Fitness", "Wellness", "Martial Arts"


    }
} 