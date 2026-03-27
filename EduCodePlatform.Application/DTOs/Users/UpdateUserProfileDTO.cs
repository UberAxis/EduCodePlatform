namespace EduCodePlatform.Application.DTOs.Users
{
    public class UpdateUserProfileDTO
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
