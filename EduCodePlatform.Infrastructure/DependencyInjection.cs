using EduCodePlatform.Application.Interfaces.Auth;
using EduCodePlatform.Application.Interfaces.Repositories;
using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Infrastructure.Auth;
using EduCodePlatform.Infrastructure.Persistence;
using EduCodePlatform.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
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

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Auth
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddScoped<ITokenService, JwtTokenGenerator>();

            return services;
        }
    }
}
