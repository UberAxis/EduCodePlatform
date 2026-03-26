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

            return services;
        }
    }
}
