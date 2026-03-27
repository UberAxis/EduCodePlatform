using Microsoft.AspNetCore.Http;

namespace EduCodePlatform.Application.DTOs.Users
{
    public class UpdateUserDTO
    {
        public string Name { get; set; } = null!;
        public IFormFile? Avatar { get; set; }
        public string? Password { get; set; }
    }
}
