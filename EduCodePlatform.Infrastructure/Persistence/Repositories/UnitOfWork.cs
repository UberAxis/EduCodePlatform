using EduCodePlatform.Application.Interfaces.Repositories;

namespace EduCodePlatform.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository Users { get; set; }

        public UnitOfWork(
            AppDbContext context,
            IUserRepository userRepository)
        {
            _context = context;
            Users = userRepository;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
