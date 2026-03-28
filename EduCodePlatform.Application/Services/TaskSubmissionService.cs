using AutoMapper;
using EduCodePlatform.Application.DTOs.TaskSubmissions;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Application.Services
{
    public class TaskSubmissionService : ITaskSubmissionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public TaskSubmissionService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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

        public async Task<GetTaskSubmissionDTO> CreateAsync(Guid userId, CreateTaskSubmissionDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new KeyNotFoundException("User not found");

            var task = await _unitOfWork.LessonTasks.GetByIdAsync(dto.LessonTaskId);
            if (task == null) throw new KeyNotFoundException("LessonTask not found");

            var userAnswer = dto.Answer?.Trim();
            if (userAnswer == null) throw new ArgumentException("Answer is required");

            bool isCorrect = string.Equals(userAnswer, task.ExpectedAnswer?.Trim(), StringComparison.OrdinalIgnoreCase);

            var previousSubmissions = await _unitOfWork.TaskSubmissions.GetByUserIdAsync(userId);
            bool alreadySolved = previousSubmissions.Any(s => s.LessonTaskId == dto.LessonTaskId && s.Result);

            var tasksubmission = new TaskSubmission(
                answer: userAnswer,
                result: isCorrect,
                lessonTaskId: dto.LessonTaskId,
                userId: userId
            );

            _unitOfWork.TaskSubmissions.Add(tasksubmission);

            if (isCorrect && !alreadySolved)
            {
                user.ExperiencePoints += 10;
                user.TasksCompletedCount++;
                user.Coins += 5;
                if (task.Type == TaskType.Quiz)
                    user.QuizzesPassedCount++;

                await _unitOfWork.Achievements.TryUnlockByTitleAsync(user.Id, "Первые шаги", user);

                int newLevel = (user.ExperiencePoints / 100) + 1;
                if (newLevel > user.Level) user.Level = newLevel;

                await _userManager.UpdateAsync(user);

                await CheckLessonCompletionAsync(user, task.LessonId);
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<GetTaskSubmissionDTO>(tasksubmission);
        }

        private async Task CheckLessonCompletionAsync(User user, int lessonId)
        {
            var lesson = await _unitOfWork.Lessons.GetByIdAsync(lessonId);
            if (lesson == null) return;

            var allTaskIds = lesson.LessonTasks.Select(t => t.Id).ToList();
            if (!allTaskIds.Any()) return;

            var userSubmissions = await _unitOfWork.TaskSubmissions.GetByUserIdAsync(user.Id);
            var solvedTaskIds = userSubmissions
                .Where(s => s.Result && allTaskIds.Contains(s.LessonTaskId))
                .Select(s => s.LessonTaskId)
                .Distinct()
                .ToList();

            if (solvedTaskIds.Count == allTaskIds.Count)
            {
                var progress = await _unitOfWork.Users.GetByIdWithProgressAsync(user.Id);
                var lessonProgress = progress?.LessonProgress.FirstOrDefault(p => p.LessonId == lessonId);

                if (lessonProgress == null)
                {
                    user.LessonProgress.Add(new UserLessonProgress(user.Id, lessonId, true));
                }
                else if (!lessonProgress.IsCompleted)
                {
                    lessonProgress.Complete();
                }
                await _userManager.UpdateAsync(user);
            }
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
