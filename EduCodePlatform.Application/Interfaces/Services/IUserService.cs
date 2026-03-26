using EduCodePlatform.Application.DTOs.Users;

namespace EduCodePlatform.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserDTO>> GetAllAsync();

        Task<GetUserDTO> GetByIdAsync(int id);

        Task<GetUserDTO> CreateAsync(CreateUserDTO dto);

        Task<string> LoginAsync(LoginUserDTO dto);

        Task<GetUserDTO> UpdateAsync(int id, UpdateUserDTO dto);

        Task DeleteAsync(int id);
    }
}
