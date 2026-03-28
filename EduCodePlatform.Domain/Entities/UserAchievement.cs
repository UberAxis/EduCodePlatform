using EduCodePlatform.Domain.Common;

namespace EduCodePlatform.Domain.Entities
{
    public class UserAchievement : BaseEntity
    {
        public Guid UserId { get; protected set; }
        public virtual User User { get; protected set; } = null!;

        public int AchievementId { get; protected set; }
        public virtual Achievement Achievement { get; protected set; } = null!;

        public DateTime UnlockedAt { get; protected set; }

        protected UserAchievement() { }

        public UserAchievement(Guid userId, int achievementId)
        {
            UserId = userId;
            AchievementId = achievementId;
            UnlockedAt = DateTime.UtcNow;
        }
    }
}
