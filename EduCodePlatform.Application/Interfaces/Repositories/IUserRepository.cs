using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByNameAsync(string name);

        void Add(User user);

        void Delete(User user);

        Task<bool> ExistsByNameAsync(string name);

        Task<bool> ExistsByIdAsync(int id);

        Task<bool> ExistsByNameExceptUserAsync(string name, int id);
    }
}
