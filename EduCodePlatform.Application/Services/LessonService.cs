using AutoMapper;
using EduCodePlatform.Application.DTOs.Lessons;
using EduCodePlatform.Application.Interfaces.FileStorage.Lessons;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILessonCoverImageStorage _fileStorageService;

        public LessonService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILessonCoverImageStorage fileStorageService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
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

            var imagePath = await _fileStorageService.SaveAsync(dto.CoverImage);

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

            var lessonExist = await _unitOfWork.Lessons.ExistsByTitleAsync(title);

            if (lessonExist)
                throw new InvalidOperationException("Lesson with this name already exist");

            string? oldImagePath = lesson.CoverImage;
            string? newImage = null;

            var image = dto.CoverImage;

            if (image != null)
            {
                var imagePath = await _fileStorageService.SaveAsync(image);
                newImage = imagePath;

                if (!string.IsNullOrEmpty(oldImagePath))
                    await _fileStorageService.DeleteAsync(oldImagePath);
            }

            lesson.UpdateLesson(
                title: title,
                coverImage: newImage,
                markdownContent: dto.MarkdownContent,
                orderIndex: dto.OrderIndex,
                moduleId: dto.ModuleId
            );

            await _unitOfWork.SaveChangesAsync();

            if (image != null && oldImagePath != null)
            {
                await _fileStorageService.DeleteAsync(oldImagePath);
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<GetLessonDTO>(lesson);
        }

        public async Task DeleteAsync(int id)
        {
            var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);

            if (lesson == null)
                throw new KeyNotFoundException("Lesson not found");

            var image = lesson.CoverImage;

            _unitOfWork.Lessons.Delete(lesson);
            await _unitOfWork.SaveChangesAsync();

            if (image != null)
                await _fileStorageService.DeleteAsync(image);
        }
    }
}
