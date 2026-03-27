using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EduCodePlatform.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            
            var adminExists = userManager.Users.Any(u => u.Role == UserRole.Admin);

            if (!adminExists)
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@educode.com",
                    EmailConfirmed = true,
                    Role = UserRole.Admin
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
