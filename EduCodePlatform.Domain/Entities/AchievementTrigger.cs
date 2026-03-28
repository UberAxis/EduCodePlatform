using EduCodePlatform.Domain.Common;
using EduCodePlatform.Domain.Enums;

namespace EduCodePlatform.Domain.Entities
{
    /// <summary>
    /// Defines the conditions/triggers that unlock an achievement
    /// </summary>
    public class AchievementTrigger : BaseEntity
    {
        public int AchievementId { get; set; }
        public virtual Achievement Achievement { get; set; } = null!;

        /// <summary>
        /// Type of trigger that unlocks this achievement
        /// </summary>
        public AchievementTriggerType TriggerType { get; set; }

        /// <summary>
        /// Required value for the trigger (e.g., if TriggerType is XpThreshold, this would be the XP amount)
        /// </summary>
        public int RequiredValue { get; set; }

        /// <summary>
        /// Optional: For specific module/lesson triggers, store the ID
        /// </summary>
        public int? TargetModuleId { get; set; }
        public virtual Module? TargetModule { get; set; }

        public int? TargetLessonId { get; set; }
        public virtual Lesson? TargetLesson { get; set; }

        /// <summary>
        /// Is this trigger active/enabled
        /// </summary>
        public bool IsActive { get; set; } = true;

        protected AchievementTrigger() { }

        public AchievementTrigger(
            int achievementId,
            AchievementTriggerType triggerType,
            int requiredValue,
            int? targetModuleId = null,
            int? targetLessonId = null)
        {
            AchievementId = achievementId;
            TriggerType = triggerType;
            RequiredValue = requiredValue;
            TargetModuleId = targetModuleId;
            TargetLessonId = targetLessonId;
            IsActive = true;
        }

        /// <summary>
        /// Check if user meets the trigger conditions
        /// </summary>
        public bool CheckTrigger(User user, UserLessonProgress? lessonProgress = null)
        {
            return TriggerType switch
            {
                AchievementTriggerType.XpThreshold => user.ExperiencePoints >= RequiredValue,
                AchievementTriggerType.TasksCompletedCount => user.TasksCompletedCount >= RequiredValue,
                AchievementTriggerType.QuizzesPassedCount => user.QuizzesPassedCount >= RequiredValue,
                AchievementTriggerType.LevelThreshold => user.Level >= RequiredValue,
                AchievementTriggerType.CoinsThreshold => user.Coins >= RequiredValue,
                AchievementTriggerType.FirstTaskCompletion => user.TasksCompletedCount >= 1,
                AchievementTriggerType.FirstQuizPassed => user.QuizzesPassedCount >= 1,
                _ => false
            };
        }
    }
}
