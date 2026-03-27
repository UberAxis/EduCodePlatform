using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.Modules
{
    public class UpdateModuleDTO
    {
        public string Title { get; set; } = null!;
        public IFormFile? CoverImage { get; set; }
        [MaxLength(4200)]
        public string Description { get; set; } = null!;
        public int OrderIndex { get; set; }
    }
}
