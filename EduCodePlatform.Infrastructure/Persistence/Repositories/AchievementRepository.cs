using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure.Persistence.Repositories
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly AppDbContext _context;

        public AchievementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task TryUnlockByTitleAsync(Guid userId, string title, User user)
        {
            var achievement = await _context.Achievements
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Title == title);
            if (achievement == null) return;

            var already = await _context.UserAchievements
                .AnyAsync(ua => ua.UserId == userId && ua.AchievementId == achievement.Id);
            if (already) return;

            _context.UserAchievements.Add(new UserAchievement(userId, achievement.Id));
            user.ExperiencePoints += achievement.XpReward;
        }

        public async Task<List<Achievement>> GetAllAsync()
        {
            return await _context.Achievements
                .Include(a => a.Triggers)
                .ToListAsync();
        }

        public async Task<Achievement?> GetByIdAsync(int id)
        {
            return await _context.Achievements
                .Include(a => a.Triggers)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Achievement> CreateAsync(Achievement achievement)
        {
            _context.Achievements.Add(achievement);
            await _context.SaveChangesAsync();
            return achievement;
        }

        public async Task<Achievement> UpdateAsync(Achievement achievement)
        {
            _context.Achievements.Update(achievement);
            await _context.SaveChangesAsync();
            return achievement;
        }

        public async Task DeleteAsync(int id)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement != null)
            {
                _context.Achievements.Remove(achievement);
                await _context.SaveChangesAsync();
            }
        }
    }
}
