using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Interfaces.Repositories
{
    public interface IAchievementRepository
    {
        /// <summary>Добавляет запись о достижении и бонусный XP пользователю (без SaveChanges).</summary>
        Task TryUnlockByTitleAsync(Guid userId, string title, User user);

        /// <summary>Get all achievements.</summary>
        Task<List<Achievement>> GetAllAsync();

        /// <summary>Get achievement by ID.</summary>
        Task<Achievement?> GetByIdAsync(int id);

        /// <summary>Create a new achievement.</summary>
        Task<Achievement> CreateAsync(Achievement achievement);

        /// <summary>Update an achievement.</summary>
        Task<Achievement> UpdateAsync(Achievement achievement);

        /// <summary>Delete an achievement by ID.</summary>
        Task DeleteAsync(int id);
    }
}

