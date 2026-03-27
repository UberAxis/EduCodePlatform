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

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByUserNameAsync(string userName) =>
            await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

        public void Add(User user) =>
            _context.Users.Add(user);

        public void Delete(User user) =>
            _context.Users.Remove(user);

        public async Task<bool> ExistsByUserNameAsync(string userName) =>
            await _context.Users.AnyAsync(u => u.UserName == userName);

        public async Task<bool> ExistsByIdAsync(Guid id) =>
            await _context.Users.AnyAsync(u => u.Id == id);

        public async Task<bool> ExistsByUserNameExceptUserAsync(string userName, Guid id) =>
            await _context.Users.AnyAsync(u => u.Id != id && u.UserName == userName);
    }
}
