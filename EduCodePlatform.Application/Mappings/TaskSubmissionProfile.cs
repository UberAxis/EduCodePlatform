using AutoMapper;
using EduCodePlatform.Application.DTOs.TaskSubmissions;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Mappings
{
    public class TaskSubmissionProfile : Profile
    {
        public TaskSubmissionProfile()
        {
            CreateMap<TaskSubmission, GetTaskSubmissionDTO>();
        }
    }
}
