using GymFit.BE.Models;

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
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Role UserRole { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }

    // DTO pentru PATCH - actualizare parțială (toate opționale!)
    public class UpdateUserDTO
    {
        public string? Name { get; set; }           // ✅ NULL = nu se modifică
        public string? Email { get; set; }          // ✅ NULL = nu se modifică  
        public Role? UserRole { get; set; }         // ✅ NULL = nu se modifică
        public string? PhoneNumber { get; set; }    // ✅ NULL = nu se modifică
        public DateOnly? DateOfBirth { get; set; }  // ✅ NULL = nu se modifică
    }
} 