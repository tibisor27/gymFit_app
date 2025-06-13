using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GymFit.BE.Models
{
    public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Role is required")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Role UserRole { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    [StringLength(100, MinimumLength = 9, ErrorMessage = "Phone number must be at least 9 characters long")]
    public required string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Date Of Birth is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly DateOfBirth { get; set; }
    
    // 🔗 Navigational Properties pentru relații
    public Trainer? TrainerDetails { get; set; }  // null dacă nu e trainer
    public Member? MemberDetails { get; set; }    // null dacă nu e member
}

    public enum Role
    {
        Admin = 0,     // Administrator al sălii
        Member = 1,    // Membru/Client al sălii  
        Trainer = 2    // Antrenor/Trainer
    }
}

