using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure.Persistence.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly AppDbContext _context;

        public ModuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Module>> GetAllAsync() =>
            await _context.Modules
                .Include(m => m.Lessons)
                    .ThenInclude(l => l.LessonTasks)
                .ToListAsync();

        public async Task<Module?> GetByIdAsync(int id) =>
            await _context.Modules
                .Include(m => m.Lessons)
                    .ThenInclude(l => l.LessonTasks)
                .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<Module?> GetByTitleAsync(string title) =>
            await _context.Modules.FirstOrDefaultAsync(u => u.Title == title);

        public void Add(Module module) =>
            _context.Modules.Add(module);

        public void Delete(Module module) =>
            _context.Modules.Remove(module);

        public async Task<bool> ExistsByTitleAsync(string title) =>
            await _context.Modules.AnyAsync(u => u.Title == title);

        public async Task<bool> ExistsByIdAsync(int id) =>
            await _context.Modules.AnyAsync(u => u.Id == id);
    }
}
