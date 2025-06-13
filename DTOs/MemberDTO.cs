using System.ComponentModel.DataAnnotations;

namespace GymFit.BE.DTOs
{
    // DTO simplu pentru Member - ce trimiți la frontend
    public class MemberDTO
    {
        public int Id { get; set; }
        
        // ✅ FLAT FIELDS - pentru ușurința frontend-ului
        public string MemberName { get; set; } = string.Empty;     // Direct din user.name
        public string MemberEmail { get; set; } = string.Empty;    // Direct din user.email
        public string MemberPhone { get; set; } = string.Empty;    // Direct din user.phone
        
        // 🏃 MEMBER SPECIFIC DATA
        public bool IsActive { get; set; } = true;
        
        // 📋 NESTED OBJECT - pentru detalii complete (opțional)
        public UserDTO? User { get; set; }  // Null dacă nu vrei toate detaliile
    }

    // DTO pentru crearea unui member nou
    public class CreateMemberDTO
    {
        [Required(ErrorMessage = "UserId este obligatoriu")]
        public int UserId { get; set; } // ID-ul userului care devine member
        
        public bool IsActive { get; set; } = true; // Optional, default true
    }

    // DTO pentru PATCH - actualizare parțială member (toate opționale!)
    public class UpdateMemberDTO
    {
        public bool? IsActive { get; set; }      // ✅ NULL = nu se modifică
    }

    // DTO pentru răspuns după crearea member-ului
    public class MemberResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
} 