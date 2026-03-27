using EduCodePlatform.Application.DTOs.Lessons;

namespace EduCodePlatform.Application.Interfaces.Services
{
    public interface ILessonService
    {
        Task<IEnumerable<GetLessonDTO>> GetAllAsync();

        Task<GetLessonDTO> GetByIdAsync(int id);

        Task<GetLessonDTO> CreateAsync(CreateLessonDTO dto);

        Task<GetLessonDTO> UpdateAsync(int id, UpdateLessonDTO dto);

        Task DeleteAsync(int id);
    }
}
