using EduCodePlatform.Domain.Enums;

namespace EduCodePlatform.Application.DTOs.Users
{
    public class AdminUpdateUserDTO
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public UserRole? Role { get; set; }
        public string? NewPassword { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int? ExperiencePoints { get; set; }
        public int? Level { get; set; }
        public int? Coins { get; set; }
    }
}
