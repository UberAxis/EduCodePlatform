using EduCodePlatform.Domain.Enums;

namespace EduCodePlatform.Application.DTOs.Achievements
{
    /// <summary>
    /// DTO for displaying achievement details
    /// </summary>
    public class GetAchievementDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public int XpReward { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<GetAchievementTriggerDTO> Triggers { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating a new achievement
    /// </summary>
    public class CreateAchievementDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public int XpReward { get; set; }
    }

    /// <summary>
    /// DTO for updating an achievement
    /// </summary>
    public class UpdateAchievementDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public int? XpReward { get; set; }
    }

    /// <summary>
    /// DTO for achievement trigger configuration
    /// </summary>
    public class GetAchievementTriggerDTO
    {
        public int Id { get; set; }
        public int AchievementId { get; set; }
        public AchievementTriggerType TriggerType { get; set; }
        public int RequiredValue { get; set; }
        public int? TargetModuleId { get; set; }
        public int? TargetLessonId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// DTO for creating/updating achievement trigger
    /// </summary>
    public class CreateAchievementTriggerDTO
    {
        public int AchievementId { get; set; }
        public AchievementTriggerType TriggerType { get; set; }
        public int RequiredValue { get; set; }
        public int? TargetModuleId { get; set; }
        public int? TargetLessonId { get; set; }
    }

    /// <summary>
    /// DTO for updating achievement trigger
    /// </summary>
    public class UpdateAchievementTriggerDTO
    {
        public AchievementTriggerType? TriggerType { get; set; }
        public int? RequiredValue { get; set; }
        public int? TargetModuleId { get; set; }
        public int? TargetLessonId { get; set; }
        public bool? IsActive { get; set; }
    }
}
