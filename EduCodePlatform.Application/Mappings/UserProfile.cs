using AutoMapper;
using EduCodePlatform.Application.DTOs.Users;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAchievement, GetUserAchievementDTO>()
                .ForMember(d => d.AchievementId, o => o.MapFrom(s => s.AchievementId))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Achievement != null ? s.Achievement.Title : string.Empty))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Achievement != null ? s.Achievement.Description : string.Empty))
                .ForMember(d => d.IconUrl, o => o.MapFrom(s => s.Achievement != null ? s.Achievement.IconUrl : string.Empty))
                .ForMember(d => d.XpReward, o => o.MapFrom(s => s.Achievement != null ? s.Achievement.XpReward : 0))
                .ForMember(d => d.UnlockedAt, o => o.MapFrom(s => s.UnlockedAt));

            CreateMap<User, GetUserDTO>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.AvatarUrl, o => o.MapFrom(s => s.AvatarUrl))
                .ForMember(d => d.Role, o => o.MapFrom(s => s.Role.ToString()))
                .ForMember(d => d.ExperiencePoints, o => o.MapFrom(s => s.ExperiencePoints))
                .ForMember(d => d.Level, o => o.MapFrom(s => s.Level))
                .ForMember(d => d.Coins, o => o.MapFrom(s => s.Coins))
                .ForMember(d => d.TasksCompletedCount, o => o.MapFrom(s => s.TasksCompletedCount))
                .ForMember(d => d.QuizzesPassedCount, o => o.MapFrom(s => s.QuizzesPassedCount))
                .ForMember(d => d.LessonsCompletedCount, o => o.MapFrom(s => s.LessonProgress.Count(p => p.IsCompleted)))
                .ForMember(d => d.UnlockedAchievements, o => o.MapFrom(s => s.Achievements.Where(a => a.Achievement != null)));
        }
    }
}
