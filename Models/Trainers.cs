using System.ComponentModel.DataAnnotations;
using GymFit.BE.Models;

namespace GymFit.BE.Models
{
    public class Trainers
{
    public int Id { get; set; }
    [Required(ErrorMessage = "User is required")]
    public int UserId { get; set; }

        [Required(ErrorMessage = "User is required")]
    public required User User { get; set; }

    [Required(ErrorMessage = "Specialization is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Specialization must be between 2 and 100 characters")]
    public required GymClass GymClass { get; set; }

    [Required(ErrorMessage = "Availability is required")]
    [RegularExpression(@"^([A-Za-z]+:\s*[0-9]{1,2}:[0-9]{2}\s*-\s*[0-9]{1,2}:[0-9]{2}\s*,\s*)*([A-Za-z]+:\s*[0-9]{1,2}:[0-9]{2}\s*-\s*[0-9]{1,2}:[0-9]{2})$",
        ErrorMessage = "Availability must be in format 'Day: HH:MM-HH:MM, Day: HH:MM-HH:MM'")]
    public required string Availability { get; set; }

    [Required(ErrorMessage = "TimeSlots is required")]
    public required ICollection<TimeSlot> TimeSlots { get; set; }
    }
}