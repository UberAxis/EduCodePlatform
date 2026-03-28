using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.Users
{
    public class CreateUserDTO
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
