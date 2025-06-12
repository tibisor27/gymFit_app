namespace GymFit.BE.DTOs
{
    // DTO simplu pentru Trainer - ce trimiți la frontend
    public class TrainerDTO
    {
        public int Id { get; set; }
        
        // DIRECT din User - FĂRĂ nested object!
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        
        // Info despre trainer
        public string Experience { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty;
    }

    // DTO pentru crearea unui trainer nou
    public class CreateTrainerDTO
    {
        public int UserId { get; set; } // ID-ul userului care devine trainer
        public string Experience { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty;
    }
} 