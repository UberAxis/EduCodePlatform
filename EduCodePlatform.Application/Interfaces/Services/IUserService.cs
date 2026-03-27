using EduCodePlatform.Application.DTOs.Users;

namespace EduCodePlatform.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserDTO>> GetAllAsync();

        Task<GetUserDTO> GetByIdAsync(Guid id);

        Task<GetUserDTO> CreateAsync(CreateUserDTO dto);

        Task<string> LoginAsync(LoginUserDTO dto);

        Task<GetUserDTO> UpdateProfileAsync(Guid userId, UpdateUserProfileDTO dto);

        Task<GetUserDTO> AdminUpdateUserAsync(Guid userId, AdminUpdateUserDTO dto);

        Task DeleteAsync(Guid id);
    }
}
