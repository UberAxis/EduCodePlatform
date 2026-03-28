using AutoMapper;
using EduCodePlatform.Application.DTOs.LessonTasks;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Mappings
{
    public class LessonTaskProfile : Profile
    {
        public LessonTaskProfile()
        {
            CreateMap<QuizOption, QuizOptionDTO>();

            CreateMap<LessonTask, GetLessonTaskDTO>()
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.QuizOptions, o => o.MapFrom(s => s.QuizOptions));
        }
    }
}
