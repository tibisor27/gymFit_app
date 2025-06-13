using GymFit.BE.Models;
using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.DTOs
{
    // DTO simplu pentru User - ce trimiți la frontend (FĂRĂ PAROLĂ!)
    public class UserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^(\+4|0)[0-9]{9}$", ErrorMessage = "Phone number must be in Romanian format")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public Role UserRole { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateOnly DateOfBirth { get; set; }
    }

    // DTO pentru login - ce primești de la frontend
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;
    }

    // DTO pentru crearea unui user nou - ce primești de la frontend
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s-']+$", ErrorMessage = "Name can only contain letters, spaces, hyphens and apostrophes")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Role is required")]
        public Role UserRole { get; set; }
        
        [Required(ErrorMessage = "Date Of Birth is required")]
        [CustomValidation(typeof(DateOfBirthValidator), "ValidateDateOfBirth")]
        public DateOnly DateOfBirth { get; set; }
    }

    public static class DateOfBirthValidator
    {
        public static ValidationResult ValidateDateOfBirth(DateOnly dateOfBirth, ValidationContext context)
        {
            var minimumAge = 16;
            var maximumAge = 100;
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dateOfBirth.Year;
            
            if (dateOfBirth > today.AddYears(-age)) age--;
            
            if (age < minimumAge)
                return new ValidationResult($"User must be at least {minimumAge} years old");
            
            if (age > maximumAge)
                return new ValidationResult($"User cannot be older than {maximumAge} years");
            
            return ValidationResult.Success!;
        }
    }

    // DTO pentru PATCH - actualizare parțială (toate opționale!)
    public class UpdateUserDTO
    {
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s-']+$", ErrorMessage = "Name can only contain letters, spaces, hyphens and apostrophes")]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        public Role? UserRole { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^(\+4|0)[0-9]{9}$", ErrorMessage = "Phone number must be in Romanian format")]
        public string? PhoneNumber { get; set; }

        [CustomValidation(typeof(DateOfBirthValidator), "ValidateDateOfBirth")]
        public DateOnly? DateOfBirth { get; set; }
    }
} 