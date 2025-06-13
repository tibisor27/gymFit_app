using GymFit.BE.Models;
using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.DTOs
{
    // DTO simplu pentru User - ce trimiți la frontend (FĂRĂ PAROLĂ!)
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Role UserRole { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }

    // DTO pentru login - ce primești de la frontend
    public class LoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // DTO pentru crearea unui user nou - ce primești de la frontend
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(100, MinimumLength = 9, ErrorMessage = "Phone number must be at least 9 characters long")]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Role is required")]
        public Role UserRole { get; set; }
        
        [Required(ErrorMessage = "Date Of Birth is required")]
        public DateOnly DateOfBirth { get; set; }
    }

    // DTO pentru PATCH - actualizare parțială (toate opționale!)
    public class UpdateUserDTO
    {
        [MinLength(2, ErrorMessage = "Numele trebuie să aibă minim 2 caractere")]
        [MaxLength(100, ErrorMessage = "Numele nu poate depăși 100 caractere")]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Format email invalid")]
        public string? Email { get; set; }

        public Role? UserRole { get; set; }

        [Phone(ErrorMessage = "Format număr telefon invalid")]
        [RegularExpression(@"^(\+4|0)[0-9]{9}$", ErrorMessage = "Numărul de telefon trebuie să fie format românesc")]
        public string? PhoneNumber { get; set; }

        public DateOnly? DateOfBirth { get; set; }
    }
} 