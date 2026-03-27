using EduCodePlatform.Application.DTOs.LessonTasks;

namespace EduCodePlatform.Application.Interfaces.Services
{
    public interface ILessonTaskService
    {
        Task<IEnumerable<GetLessonTaskDTO>> GetAllAsync();

        Task<GetLessonTaskDTO> GetByIdAsync(int id);

        Task<GetLessonTaskDTO> CreateAsync(CreateLessonTaskDTO dto);

        Task<GetLessonTaskDTO> UpdateAsync(int id, UpdateLessonTaskDTO dto);

        Task DeleteAsync(int id);
    }
}
