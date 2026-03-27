using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Interfaces.Repositories
{
    public interface ILessonRepository
    {
        Task<IEnumerable<Lesson>> GetAllAsync();

        Task<Lesson?> GetByIdAsync(int id);

        Task<Lesson?> GetByTitleAsync(string title);

        void Add(Lesson Lesson);

        void Delete(Lesson Lesson);

        Task<bool> ExistsByTitleAsync(string title);

        Task<bool> ExistsByIdAsync(int id);
    }
}
