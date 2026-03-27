using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure.Persistence.Repositories
{
    public class TaskSubmissionRepository : ITaskSubmissionRepository
    {
        private readonly AppDbContext _context;

        public TaskSubmissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskSubmission>> GetAllAsync() =>
            await _context.TaskSubmissions.ToListAsync();

        public async Task<TaskSubmission?> GetByIdAsync(int id) =>
            await _context.TaskSubmissions.FirstOrDefaultAsync(u => u.Id == id);

        public void Add(TaskSubmission tasksubmission) =>
            _context.TaskSubmissions.Add(tasksubmission);

        public void Delete(TaskSubmission tasksubmission) =>
            _context.TaskSubmissions.Remove(tasksubmission);

        public async Task<bool> ExistsByIdAsync(int id) =>
            await _context.TaskSubmissions.AnyAsync(u => u.Id == id);
    }
}
