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
                 .ForMember(d => d.Name,
                 o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Role,
                o => o.MapFrom(s => s.Role.ToString()));
        }
    }
}
