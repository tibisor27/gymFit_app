using System.ComponentModel.DataAnnotations;

namespace GymFit.API.Models
{
    public class GymClass
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        public int TrainerId { get; set; }
        
        public int MaxCapacity { get; set; }
        
        public int Duration { get; set; } // minutes
        
        public decimal Price { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation property
        public virtual User? Trainer { get; set; }
    }
} 