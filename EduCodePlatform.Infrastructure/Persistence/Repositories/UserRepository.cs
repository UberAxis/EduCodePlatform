using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByNameAsync(string name) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Name == name);

        public void Add(User user) =>
            _context.Users.Add(user);

        public void Delete(User user) =>
            _context.Users.Remove(user);

        public async Task<bool> ExistsByNameAsync(string name) =>
            await _context.Users.AnyAsync(u => u.Name == name);

        public async Task<bool> ExistsByIdAsync(int id) =>
            await _context.Users.AnyAsync(u => u.Id == id);

        public async Task<bool> ExistsByNameExceptUserAsync(string name, int id) =>
            await _context.Users.AnyAsync(u => u.Id != id && u.Name == name);
    }
}
