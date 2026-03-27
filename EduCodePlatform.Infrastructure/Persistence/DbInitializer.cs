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
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            string[] roles = { "Admin", "User" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }

            var adminExists = userManager.Users.Any(u => u.Role == UserRole.Admin);

            if (!adminExists)
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@educode.com",
                    EmailConfirmed = true,
                    Role = UserRole.Admin,
                    FullName = "System Administrator"
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
