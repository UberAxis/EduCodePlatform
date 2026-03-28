using EduCodePlatform.Domain.Common;

namespace EduCodePlatform.Domain.Entities
{
    public class Achievement : BaseEntity
    {
        public string Title { get; protected set; } = null!;
        public string Description { get; protected set; } = null!;
        public string IconUrl { get; protected set; } = null!;
        public int XpReward { get; protected set; }

        public virtual ICollection<UserAchievement> UserAchievements { get; } = new List<UserAchievement>();
        public virtual ICollection<AchievementTrigger> Triggers { get; } = new List<AchievementTrigger>();

        protected Achievement() { }

        public Achievement(string title, string description, string iconUrl, int xpReward)
        {
            Title = title;
            Description = description;
            IconUrl = iconUrl;
            XpReward = xpReward;
        }
    }
}
