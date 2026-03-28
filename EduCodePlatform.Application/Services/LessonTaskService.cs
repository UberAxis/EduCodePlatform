using AutoMapper;
using EduCodePlatform.Application.DTOs.LessonTasks;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Services
{
    public class LessonTaskService : ILessonTaskService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LessonTaskService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetLessonTaskDTO>> GetAllAsync()
        {
            var lessontasks = await _unitOfWork.LessonTasks.GetAllAsync();
            return _mapper.Map<IEnumerable<GetLessonTaskDTO>>(lessontasks);
        }

        public async Task<GetLessonTaskDTO> GetByIdAsync(int id)
        {
            var lessontask = await _unitOfWork.LessonTasks.GetByIdAsync(id);

            if (lessontask == null)
                throw new KeyNotFoundException("LessonTask not found");

            return _mapper.Map<GetLessonTaskDTO>(lessontask);
        }

        public async Task<GetLessonTaskDTO> CreateAsync(CreateLessonTaskDTO dto)
        {
            var title = dto.Title;

            var lessontaskExist = await _unitOfWork.LessonTasks.ExistsByTitleAsync(title);

            if (lessontaskExist)
                throw new InvalidOperationException("LessonTask with this name already exist");

            var lessontask = new LessonTask(
                title: title,
                markdownContent: dto.MarkdownContent,
                expectedAnswer: dto.ExpectedAnswer,
                lessonId: dto.LessonId,
                type: dto.Type,
                initialCode: dto.InitialCode,
                correctAnswerIndex: dto.CorrectAnswerIndex
            );

            if (dto.Type.ToString() == "Quiz" && dto.QuizOptions != null && dto.QuizOptions.Count > 0)
            {
                foreach (var option in dto.QuizOptions)
                {
                    lessontask.QuizOptions.Add(new QuizOption(option.Text));
                }
            }

            _unitOfWork.LessonTasks.Add(lessontask);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GetLessonTaskDTO>(lessontask);
        }

        public async Task<GetLessonTaskDTO> UpdateAsync(int id, UpdateLessonTaskDTO dto)
        {
            var lessontask = await _unitOfWork.LessonTasks.GetByIdAsync(id);

            if (lessontask == null)
                throw new KeyNotFoundException("LessonTask not found");

            var title = dto.Title;

            var lessontaskExist = await _unitOfWork.LessonTasks.ExistsByTitleAsync(title, id);

            if (lessontaskExist)
                throw new InvalidOperationException("LessonTask with this name already exist");

            lessontask.QuizOptions.Clear();

            lessontask.UpdateLessonTask(
                title: title,
                markdownContent: dto.MarkdownContent,
                expectedAnswer: dto.ExpectedAnswer,
                lessonId: dto.LessonId,
                type: dto.Type,
                initialCode: dto.InitialCode,
                correctAnswerIndex: dto.CorrectAnswerIndex
            );

            if (dto.Type.ToString() == "Quiz" && dto.QuizOptions != null && dto.QuizOptions.Count > 0)
            {
                foreach (var option in dto.QuizOptions)
                {
                    lessontask.QuizOptions.Add(new QuizOption(option.Text));
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<GetLessonTaskDTO>(lessontask);
        }

        public async Task DeleteAsync(int id)
        {
            var lessontask = await _unitOfWork.LessonTasks.GetByIdAsync(id);

            if (lessontask == null)
                throw new KeyNotFoundException("LessonTask not found");

            _unitOfWork.LessonTasks.Delete(lessontask);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
