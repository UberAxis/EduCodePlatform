using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.Lessons
{
    public class CreateLessonDTO
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public IFormFile CoverImage { get; set; } = null!;
        [Required]
        [MaxLength(4200)]
        public string MarkdownContent { get; set; } = null!;
        [Required]
        public int OrderIndex { get; set; }
        [Required]
        public int ModuleId { get; set; }
    }
}
