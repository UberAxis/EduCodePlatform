using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure.Persistence.Repositories
{
    public class LessonTaskRepository : ILessonTaskRepository
    {
        private readonly AppDbContext _context;

        public LessonTaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LessonTask>> GetAllAsync() =>
            await _context.LessonTasks.ToListAsync();

        public async Task<LessonTask?> GetByIdAsync(int id) =>
            await _context.LessonTasks.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<LessonTask?> GetByTitleAsync(string title) =>
            await _context.LessonTasks.FirstOrDefaultAsync(u => u.Title == title);

        public void Add(LessonTask lessontask) =>
            _context.LessonTasks.Add(lessontask);

        public void Delete(LessonTask lessontask) =>
            _context.LessonTasks.Remove(lessontask);

        public async Task<bool> ExistsByTitleAsync(string title) =>
            await _context.LessonTasks.AnyAsync(u => u.Title == title);

        public async Task<bool> ExistsByIdAsync(int id) =>
            await _context.LessonTasks.AnyAsync(u => u.Id == id);
    }
}
