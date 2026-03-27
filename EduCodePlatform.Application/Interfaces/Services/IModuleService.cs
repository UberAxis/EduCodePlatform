using EduCodePlatform.Application.DTOs.Modules;

namespace EduCodePlatform.Application.Interfaces.Services
{
    public interface IModuleService
    {
        Task<IEnumerable<GetModuleDTO>> GetAllAsync();

        Task<GetModuleDTO> GetByIdAsync(int id);

        Task<GetModuleDTO> CreateAsync(CreateModuleDTO dto);

        Task<GetModuleDTO> UpdateAsync(int id, UpdateModuleDTO dto);

        Task DeleteAsync(int id);
    }
}
