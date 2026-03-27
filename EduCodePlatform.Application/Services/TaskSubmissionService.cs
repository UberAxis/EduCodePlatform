using AutoMapper;
using EduCodePlatform.Application.DTOs.TaskSubmissions;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Services
{
    public class TaskSubmissionService : ITaskSubmissionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TaskSubmissionService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetTaskSubmissionDTO>> GetAllAsync()
        {
            var tasksubmissions = await _unitOfWork.TaskSubmissions.GetAllAsync();
            return _mapper.Map<IEnumerable<GetTaskSubmissionDTO>>(tasksubmissions);
        }

        public async Task<GetTaskSubmissionDTO> GetByIdAsync(int id)
        {
            var tasksubmission = await _unitOfWork.TaskSubmissions.GetByIdAsync(id);

            if (tasksubmission == null)
                throw new KeyNotFoundException("TaskSubmission not found");

            return _mapper.Map<GetTaskSubmissionDTO>(tasksubmission);
        }

        public async Task<GetTaskSubmissionDTO> CreateAsync(CreateTaskSubmissionDTO dto)
        {
            var taskId = dto.LessonTaskId;

            var task = await _unitOfWork.LessonTasks.GetByIdAsync(taskId);

            if (task == null)
                throw new KeyNotFoundException("LessonTask not found");

            var userAnswer = dto.Answer?.Trim();

            if (userAnswer == null)
                throw new ArgumentException("Answer is required");

            var expectedAnswer = task.ExpectedAnswer?.Trim();

            bool result = string.Equals(
                userAnswer,
                expectedAnswer,
                StringComparison.OrdinalIgnoreCase
            );

            var tasksubmission = new TaskSubmission(
                answer: userAnswer,
                result: result,
                lessonTaskId: dto.LessonTaskId
            );

            _unitOfWork.TaskSubmissions.Add(tasksubmission);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GetTaskSubmissionDTO>(tasksubmission);
        }

        public async Task DeleteAsync(int id)
        {
            var tasksubmission = await _unitOfWork.TaskSubmissions.GetByIdAsync(id);

            if (tasksubmission == null)
                throw new KeyNotFoundException("TaskSubmission not found");

            _unitOfWork.TaskSubmissions.Delete(tasksubmission);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
