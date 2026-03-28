using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Infrastructure.Persistence.Repositories
{
    public interface IAchievementTriggerRepository
    {
        Task<List<AchievementTrigger>> GetAllAsync();
        Task<AchievementTrigger?> GetByIdAsync(int id);
        Task<List<AchievementTrigger>> GetByAchievementIdAsync(int achievementId);
        Task<AchievementTrigger> CreateAsync(AchievementTrigger trigger);
        Task<AchievementTrigger> UpdateAsync(AchievementTrigger trigger);
        Task DeleteAsync(int id);
        Task<List<AchievementTrigger>> GetActiveTriggersByTypeAsync(string triggerType);
    }
}
