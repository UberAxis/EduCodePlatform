using AutoMapper;
using EduCodePlatform.Application.DTOs.Achievements;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Mappings
{
    public class AchievementProfile : Profile
    {
        public AchievementProfile()
        {
            CreateMap<Achievement, GetAchievementDTO>()
                .ForMember(d => d.Triggers,
                    o => o.MapFrom(s => s.Triggers));

            CreateMap<AchievementTrigger, GetAchievementTriggerDTO>();
        }
    }
}
