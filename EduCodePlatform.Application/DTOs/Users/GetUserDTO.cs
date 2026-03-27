namespace EduCodePlatform.Application.DTOs.Users
{
    public class GetUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = null!;
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public int Coins { get; set; }
        public int TasksCompletedCount { get; set; }
        public int QuizzesPassedCount { get; set; }
    }
}
