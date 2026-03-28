using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Interfaces.Repositories
{
    public interface ITaskSubmissionRepository
    {
        Task<IEnumerable<TaskSubmission>> GetAllAsync();

        Task<TaskSubmission?> GetByIdAsync(int id);

        Task<IEnumerable<TaskSubmission>> GetByUserIdAsync(Guid userId);

        void Add(TaskSubmission TaskSubmission);

        void Delete(TaskSubmission TaskSubmission);

        Task<bool> ExistsByIdAsync(int id);
    }
}
