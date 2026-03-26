using EduCodePlatform.Application.Interfaces.Auth;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Infrastructure.Auth;
using EduCodePlatform.Infrastructure.Persistence;
using EduCodePlatform.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Auth
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, JwtTokenGenerator>();

            return services;
        }
    }
}
