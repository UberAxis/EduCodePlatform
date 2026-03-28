using EduCodePlatform.Application.DTOs.Users;

namespace EduCodePlatform.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserDTO>> GetAllAsync();

        Task<GetUserDTO> GetByIdAsync(Guid id);

        Task<GetUserDTO> GetMeAsync(Guid userId);

        Task<GetUserDTO> CreateAsync(CreateUserDTO dto);

        Task<LoginResponseDTO> LoginAsync(LoginUserDTO dto);

        Task<GetUserDTO> UpdateProfileAsync(Guid userId, UpdateUserProfileDTO dto);

        Task<GetUserDTO> AdminUpdateUserAsync(Guid userId, AdminUpdateUserDTO dto);

        Task LinkChildAsync(Guid parentId, string childUserName);

        Task<string> GenerateLinkCodeAsync(Guid userId);

        Task LinkChildByCodeAsync(Guid parentId, string code);

        Task UnlinkChildAsync(Guid parentId, Guid childId);

        Task<IEnumerable<GetUserDTO>> GetChildrenAsync(Guid parentId);

        Task<IEnumerable<GetUserDTO>> GetLeaderboardAsync(int count);

        Task DeleteAsync(Guid id);
    }
}
