using EduCodePlatform.Application.Interfaces.Auth;
using EduCodePlatform.Application.Interfaces.FileStorage.Lessons;
using EduCodePlatform.Application.Interfaces.FileStorage.Modules;
using EduCodePlatform.Application.Interfaces.FileStorage.Users;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Infrastructure.Auth;
using EduCodePlatform.Infrastructure.FileStorage;
using EduCodePlatform.Infrastructure.FileStorage.Lessons;
using EduCodePlatform.Infrastructure.FileStorage.Modules;
using EduCodePlatform.Infrastructure.FileStorage.Users;
using EduCodePlatform.Infrastructure.Persistence;
using EduCodePlatform.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EduCodePlatform.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Identity
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ILessonTaskRepository, LessonTaskRepository>();
            services.AddScoped<ITaskSubmissionRepository, TaskSubmissionRepository>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Auth
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddScoped<ITokenService, JwtTokenGenerator>();

            // File storage
            services.Configure<FileStorageOptions>(configuration.GetSection("FileStorage"));
            services.AddScoped<IModuleImageStorage, ModuleFileStorageService>();
            services.AddScoped<ILessonCoverImageStorage, LessonCoverImageService>();
            services.AddScoped<IUserImageStorage, UserImageService>();

            return services;
        }
    }
}
