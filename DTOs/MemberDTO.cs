namespace GymFit.BE.DTOs
{
    // DTO simplu pentru Member - ce trimiți la frontend
    public class MemberDTO
    {
        public int Id { get; set; }
        
        // DIRECT din User - FĂRĂ nested object!
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        
        // Info despre member
        public bool IsActive { get; set; }
        public DateTime JoinedAt { get; set; }
    }

    // DTO pentru crearea unui member nou
    public class CreateMemberDTO
    {
        public int UserId { get; set; } // ID-ul userului care devine member
    }
} 