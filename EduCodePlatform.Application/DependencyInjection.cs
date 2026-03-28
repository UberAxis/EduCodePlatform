using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EduCodePlatform.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ILessonTaskService, LessonTaskService>();
            services.AddScoped<ITaskSubmissionService, TaskSubmissionService>();
            services.AddScoped<IAchievementService, AchievementService>();

            return services;
        }
    }
}
