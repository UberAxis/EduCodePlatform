using AutoMapper;
using EduCodePlatform.Application.DTOs.Users;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Mappings.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUserDTO>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
                .ForMember(d => d.AvatarUrl, o => o.MapFrom(s => s.AvatarUrl))
                .ForMember(d => d.Role, o => o.MapFrom(s => s.Role.ToString()))
                .ForMember(d => d.ExperiencePoints, o => o.MapFrom(s => s.ExperiencePoints))
                .ForMember(d => d.Level, o => o.MapFrom(s => s.Level))
                .ForMember(d => d.Coins, o => o.MapFrom(s => s.Coins))
                .ForMember(d => d.TasksCompletedCount, o => o.MapFrom(s => s.TasksCompletedCount))
                .ForMember(d => d.QuizzesPassedCount, o => o.MapFrom(s => s.QuizzesPassedCount));
        }
    }
}
