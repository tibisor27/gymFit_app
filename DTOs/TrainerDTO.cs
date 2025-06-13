using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.DTOs
{
    // DTO simplu pentru Trainer - ce trimiți la frontend
    public class TrainerDTO
    {
        public int Id { get; set; }
        
        // ✅ FLAT FIELDS - pentru ușurința frontend-ului
        public string TrainerName { get; set; } = string.Empty;    // Direct din user.name
        public string TrainerEmail { get; set; } = string.Empty;   // Direct din user.email
        public string TrainerPhone { get; set; } = string.Empty;   // Direct din user.phone
        
        // 🏋️ TRAINER SPECIFIC DATA
        public string Experience { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty;
        
        // 📋 NESTED OBJECT - pentru detalii complete (opțional)
        public UserDTO? User { get; set; }  // Null dacă nu vrei toate detaliile
    }

    // DTO pentru crearea unui trainer nou
    public class CreateTrainerDTO
    {
        [Required(ErrorMessage = "UserId este obligatoriu")]
        public int UserId { get; set; } // ID-ul userului care devine trainer
        
        [Required(ErrorMessage = "Experiența este obligatorie")]
        [MinLength(5, ErrorMessage = "Experiența trebuie să aibă minim 5 caractere")]
        [MaxLength(200, ErrorMessage = "Experiența nu poate depăși 200 caractere")]
        public string Experience { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Introducerea este obligatorie")]
        [MinLength(10, ErrorMessage = "Introducerea trebuie să aibă minim 10 caractere")]
        [MaxLength(500, ErrorMessage = "Introducerea nu poate depăși 500 caractere")]
        public string Introduction { get; set; } = string.Empty;
    }

    // DTO pentru PATCH - actualizare parțială trainer (toate opționale!)
    public class UpdateTrainerDTO
    {
        [MinLength(5, ErrorMessage = "Experiența trebuie să aibă minim 5 caractere")]
        [MaxLength(200, ErrorMessage = "Experiența nu poate depăși 200 caractere")]
        public string? Experience { get; set; }        // ✅ NULL = nu se modifică

        [MinLength(10, ErrorMessage = "Introducerea trebuie să aibă minim 10 caractere")]
        [MaxLength(500, ErrorMessage = "Introducerea nu poate depăși 500 caractere")]
        public string? Introduction { get; set; }      // ✅ NULL = nu se modifică
    }

    // DTO pentru răspuns după crearea trainer-ului
    public class TrainerResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Experience { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
} 