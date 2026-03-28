using EduCodePlatform.Application.DTOs.Modules;

namespace EduCodePlatform.Application.Interfaces.Services
{
    public interface IModuleService
    {
        Task<IEnumerable<GetModuleDTO>> GetAllAsync(Guid? forUserId = null);

        Task<GetModuleDTO> GetByIdAsync(int id, Guid? forUserId = null);

        Task<GetModuleDTO> CreateAsync(CreateModuleDTO dto);

        Task<GetModuleDTO> UpdateAsync(int id, UpdateModuleDTO dto);

        Task DeleteAsync(int id);
    }
}
