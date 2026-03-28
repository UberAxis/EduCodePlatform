namespace EduCodePlatform.Application.DTOs.Users
{
    public class GetUserAchievementDTO
    {
        public int AchievementId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public int XpReward { get; set; }
        public DateTime UnlockedAt { get; set; }
    }
}
