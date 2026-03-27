using EduCodePlatform.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Infrastructure.Persistence.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AppDbContext _context;

        public LessonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lesson>> GetAllAsync() =>
            await _context.Lessons.ToListAsync();

        public async Task<Lesson?> GetByIdAsync(int id) =>
            await _context.Lessons.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<Lesson?> GetByTitleAsync(string title) =>
            await _context.Lessons.FirstOrDefaultAsync(u => u.Title == title);

        public void Add(Lesson lesson) =>
            _context.Lessons.Add(lesson);

        public void Delete(Lesson lesson) =>
            _context.Lessons.Remove(lesson);

        public async Task<bool> ExistsByTitleAsync(string title) =>
            await _context.Lessons.AnyAsync(u => u.Title == title);

        public async Task<bool> ExistsByIdAsync(int id) =>
            await _context.Lessons.AnyAsync(u => u.Id == id);
    }
}
