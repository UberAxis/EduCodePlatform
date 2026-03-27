using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Interfaces.Repositories
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAllAsync();

        Task<Module?> GetByIdAsync(int id);

        Task<Module?> GetByTitleAsync(string title);

        void Add(Module Module);

        void Delete(Module Module);

        Task<bool> ExistsByTitleAsync(string title);

        Task<bool> ExistsByIdAsync(int id);
    }
}
