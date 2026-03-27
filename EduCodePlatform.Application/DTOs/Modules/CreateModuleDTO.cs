using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.Modules
{
    public class CreateModuleDTO
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public IFormFile CoverImage { get; set; } = null!;
        [Required]
        [MaxLength(4200)]
        public string Description { get; set; } = null!;
        [Required]
        public int OrderIndex { get; set; }
    }
}
