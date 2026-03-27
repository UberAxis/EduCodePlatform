using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByUserNameAsync(string userName);

        void Add(User user);

        void Delete(User user);

        Task<bool> ExistsByUserNameAsync(string userName);

        Task<bool> ExistsByIdAsync(Guid id);

        Task<bool> ExistsByUserNameExceptUserAsync(string userName, Guid id);
    }
}
