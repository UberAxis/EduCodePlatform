using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.Users
{
    public class CreateUserDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public IFormFile Avatar { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
