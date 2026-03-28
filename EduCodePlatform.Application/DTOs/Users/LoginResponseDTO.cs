namespace EduCodePlatform.Application.DTOs.Users
{
    public class LoginResponseDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Token { get; set; } = null!;
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public string Role { get; set; } = null!;
    }
}
