using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure.Persistence.Repositories
{
    public class AchievementTriggerRepository : IAchievementTriggerRepository
    {
        private readonly AppDbContext _context;

        public AchievementTriggerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AchievementTrigger>> GetAllAsync()
        {
            return await _context.AchievementTriggers
                .Include(at => at.Achievement)
                .Include(at => at.TargetModule)
                .Include(at => at.TargetLesson)
                .OrderBy(at => at.AchievementId)
                .ToListAsync();
        }

        public async Task<AchievementTrigger?> GetByIdAsync(int id)
        {
            return await _context.AchievementTriggers
                .Include(at => at.Achievement)
                .Include(at => at.TargetModule)
                .Include(at => at.TargetLesson)
                .FirstOrDefaultAsync(at => at.Id == id);
        }

        public async Task<List<AchievementTrigger>> GetByAchievementIdAsync(int achievementId)
        {
            return await _context.AchievementTriggers
                .Where(at => at.AchievementId == achievementId)
                .Include(at => at.TargetModule)
                .Include(at => at.TargetLesson)
                .ToListAsync();
        }

        public async Task<AchievementTrigger> CreateAsync(AchievementTrigger trigger)
        {
            _context.AchievementTriggers.Add(trigger);
            await _context.SaveChangesAsync();
            return trigger;
        }

        public async Task<AchievementTrigger> UpdateAsync(AchievementTrigger trigger)
        {
            _context.AchievementTriggers.Update(trigger);
            await _context.SaveChangesAsync();
            return trigger;
        }

        public async Task DeleteAsync(int id)
        {
            var trigger = await _context.AchievementTriggers.FindAsync(id);
            if (trigger != null)
            {
                _context.AchievementTriggers.Remove(trigger);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AchievementTrigger>> GetActiveTriggersByTypeAsync(string triggerType)
        {
            return await _context.AchievementTriggers
                .Where(at => at.IsActive && at.TriggerType.ToString() == triggerType)
                .Include(at => at.Achievement)
                .ToListAsync();
        }
    }
}

