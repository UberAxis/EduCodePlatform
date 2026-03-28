using AutoMapper;
using EduCodePlatform.Application.DTOs.Modules;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Mappings
{
    public class ModuleProfile : Profile
    {
        public ModuleProfile()
        {
            CreateMap<Module, GetModuleDTO>()
                 .ForMember(d => d.Title,
                 o => o.MapFrom(s => s.Title))
                 .ForMember(d => d.CoverImage,
                 o => o.MapFrom(s => s.CoverImage))
                 .ForMember(d => d.Description,
                 o => o.MapFrom(s => s.Description))
                 .ForMember(d => d.OrderIndex,
                 o => o.MapFrom(s => s.OrderIndex))
                 .ForMember(d => d.Lessons,
                 o => o.MapFrom(s => s.Lessons));

        }
    }
}
