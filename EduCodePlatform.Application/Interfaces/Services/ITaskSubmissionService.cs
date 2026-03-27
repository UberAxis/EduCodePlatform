using EduCodePlatform.Application.DTOs.TaskSubmissions;

namespace EduCodePlatform.Application.Interfaces.Services
{
    public interface ITaskSubmissionService
    {
        Task<IEnumerable<GetTaskSubmissionDTO>> GetAllAsync();

        Task<GetTaskSubmissionDTO> GetByIdAsync(int id);

        Task<GetTaskSubmissionDTO> CreateAsync(CreateTaskSubmissionDTO dto);

        Task DeleteAsync(int id);
    }
}
