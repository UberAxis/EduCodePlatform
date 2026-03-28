using AutoMapper;
using EduCodePlatform.Application.DTOs.Lessons;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Mappings
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            CreateMap<Lesson, GetLessonDTO>()
                 .ForMember(d => d.Title,
                 o => o.MapFrom(s => s.Title))
                 .ForMember(d => d.CoverImage,
                 o => o.MapFrom(s => s.CoverImage))
                 .ForMember(d => d.MarkdownContent,
                 o => o.MapFrom(s => s.MarkdownContent))
                 .ForMember(d => d.OrderIndex,
                 o => o.MapFrom(s => s.OrderIndex))
                 .ForMember(d => d.Task,
                 o => o.MapFrom(s => s.LessonTasks.FirstOrDefault()));

        }
    }
}
