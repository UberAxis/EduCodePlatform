using AutoMapper;
using EduCodePlatform.Application.DTOs.Achievements;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Infrastructure.Persistence.Repositories;

namespace EduCodePlatform.Application.Services
{
    public interface IAchievementService
    {
        Task<IEnumerable<GetAchievementDTO>> GetAllAsync();
        Task<GetAchievementDTO> GetByIdAsync(int id);
        Task<GetAchievementDTO> CreateAsync(CreateAchievementDTO dto);
        Task<GetAchievementDTO> UpdateAsync(int id, UpdateAchievementDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<GetAchievementTriggerDTO>> GetTriggersAsync();
        Task<GetAchievementTriggerDTO> GetTriggerByIdAsync(int id);
        Task<GetAchievementTriggerDTO> CreateTriggerAsync(CreateAchievementTriggerDTO dto);
        Task<GetAchievementTriggerDTO> UpdateTriggerAsync(int id, UpdateAchievementTriggerDTO dto);
        Task DeleteTriggerAsync(int id);
        Task<IEnumerable<GetAchievementTriggerDTO>> GetTriggersByAchievementIdAsync(int achievementId);
    }

    public class AchievementService : IAchievementService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAchievementTriggerRepository _triggerRepository;

        public AchievementService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAchievementTriggerRepository triggerRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _triggerRepository = triggerRepository;
        }

        public async Task<IEnumerable<GetAchievementDTO>> GetAllAsync()
        {
            var achievements = await _unitOfWork.Achievements.GetAllAsync();
            return _mapper.Map<List<GetAchievementDTO>>(achievements);
        }

        public async Task<GetAchievementDTO> GetByIdAsync(int id)
        {
            var achievement = await _unitOfWork.Achievements.GetByIdAsync(id);

            if (achievement == null)
                throw new KeyNotFoundException("Achievement not found");

            return _mapper.Map<GetAchievementDTO>(achievement);
        }

        public async Task<GetAchievementDTO> CreateAsync(CreateAchievementDTO dto)
        {
            var achievement = new Achievement(
                dto.Title,
                dto.Description,
                dto.IconUrl,
                dto.XpReward);

            var created = await _unitOfWork.Achievements.CreateAsync(achievement);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GetAchievementDTO>(created);
        }

        public async Task<GetAchievementDTO> UpdateAsync(int id, UpdateAchievementDTO dto)
        {
            var achievement = await _unitOfWork.Achievements.GetByIdAsync(id);

            if (achievement == null)
                throw new KeyNotFoundException("Achievement not found");

            if (!string.IsNullOrEmpty(dto.Title))
                achievement = new Achievement(
                    dto.Title ?? achievement.Title,
                    dto.Description ?? achievement.Description,
                    dto.IconUrl ?? achievement.IconUrl,
                    dto.XpReward ?? achievement.XpReward);

            var updated = await _unitOfWork.Achievements.UpdateAsync(achievement);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GetAchievementDTO>(updated);
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Achievements.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetAchievementTriggerDTO>> GetTriggersAsync()
        {
            var triggers = await _triggerRepository.GetAllAsync();
            return _mapper.Map<List<GetAchievementTriggerDTO>>(triggers);
        }

        public async Task<GetAchievementTriggerDTO> GetTriggerByIdAsync(int id)
        {
            var trigger = await _triggerRepository.GetByIdAsync(id);

            if (trigger == null)
                throw new KeyNotFoundException("Trigger not found");

            return _mapper.Map<GetAchievementTriggerDTO>(trigger);
        }

        public async Task<GetAchievementTriggerDTO> CreateTriggerAsync(CreateAchievementTriggerDTO dto)
        {
            // Validate achievement exists
            var achievement = await _unitOfWork.Achievements.GetByIdAsync(dto.AchievementId);
            if (achievement == null)
                throw new KeyNotFoundException("Achievement not found");

            var trigger = new AchievementTrigger(
                dto.AchievementId,
                dto.TriggerType,
                dto.RequiredValue,
                dto.TargetModuleId,
                dto.TargetLessonId);

            var created = await _triggerRepository.CreateAsync(trigger);
            return _mapper.Map<GetAchievementTriggerDTO>(created);
        }

        public async Task<GetAchievementTriggerDTO> UpdateTriggerAsync(int id, UpdateAchievementTriggerDTO dto)
        {
            var trigger = await _triggerRepository.GetByIdAsync(id);

            if (trigger == null)
                throw new KeyNotFoundException("Trigger not found");

            if (dto.TriggerType.HasValue)
                trigger = new AchievementTrigger(
                    trigger.AchievementId,
                    dto.TriggerType.Value,
                    dto.RequiredValue ?? trigger.RequiredValue,
                    dto.TargetModuleId ?? trigger.TargetModuleId,
                    dto.TargetLessonId ?? trigger.TargetLessonId);

            if (dto.IsActive.HasValue)
                trigger = new AchievementTrigger(
                    trigger.AchievementId,
                    trigger.TriggerType,
                    trigger.RequiredValue,
                    trigger.TargetModuleId,
                    trigger.TargetLessonId)
                { 
                    IsActive = dto.IsActive.Value 
                };

            var updated = await _triggerRepository.UpdateAsync(trigger);
            return _mapper.Map<GetAchievementTriggerDTO>(updated);
        }

        public async Task DeleteTriggerAsync(int id)
        {
            await _triggerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GetAchievementTriggerDTO>> GetTriggersByAchievementIdAsync(int achievementId)
        {
            var triggers = await _triggerRepository.GetByAchievementIdAsync(achievementId);
            return _mapper.Map<List<GetAchievementTriggerDTO>>(triggers);
        }
    }
}
