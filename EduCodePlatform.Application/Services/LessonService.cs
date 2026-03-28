using AutoMapper;
using EduCodePlatform.Application.DTOs.Lessons;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LessonService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetLessonDTO>> GetAllAsync()
        {
            var lessons = await _unitOfWork.Lessons.GetAllAsync();
            return _mapper.Map<IEnumerable<GetLessonDTO>>(lessons);
        }

        public async Task<GetLessonDTO> GetByIdAsync(int id)
        {
            var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);

            if (lesson == null)
                throw new KeyNotFoundException("Lesson not found");

            return _mapper.Map<GetLessonDTO>(lesson);
        }

        public async Task<GetLessonDTO> CreateAsync(CreateLessonDTO dto)
        {
            var title = dto.Title;

            var lessonExist = await _unitOfWork.Lessons.ExistsByTitleAsync(title);

            if (lessonExist)
                throw new InvalidOperationException("Lesson with this name already exist");

            string? imagePath = dto.CoverImage;

            var lesson = new Lesson(
                title: title,
                coverImage: imagePath,
                markdownContent: dto.MarkdownContent,
                orderIndex: dto.OrderIndex,
                moduleId: dto.ModuleId
            );

            _unitOfWork.Lessons.Add(lesson);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GetLessonDTO>(lesson);
        }

        public async Task<GetLessonDTO> UpdateAsync(int id, UpdateLessonDTO dto)
        {
            var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);

            if (lesson == null)
                throw new KeyNotFoundException("Lesson not found");

            var title = dto.Title;

            var lessonExist = await _unitOfWork.Lessons.ExistsByTitleAsync(title, id);

            if (lessonExist)
                throw new InvalidOperationException("Lesson with this name already exist");

            string? newImage = null;

            if (!string.IsNullOrEmpty(dto.CoverImage))
            {
                newImage = dto.CoverImage;
            }

            lesson.UpdateLesson(
                title: title,
                coverImage: newImage ?? lesson.CoverImage,
                markdownContent: dto.MarkdownContent,
                orderIndex: dto.OrderIndex,
                moduleId: dto.ModuleId
            );

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<GetLessonDTO>(lesson);
        }

        public async Task DeleteAsync(int id)
        {
            var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);

            if (lesson == null)
                throw new KeyNotFoundException("Lesson not found");

            _unitOfWork.Lessons.Delete(lesson);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
