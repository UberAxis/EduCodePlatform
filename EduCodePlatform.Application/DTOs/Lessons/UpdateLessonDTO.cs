using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.Lessons
{
    public class UpdateLessonDTO
    {
        public string Title { get; set; } = null!;
        public IFormFile? CoverImage { get; set; }
        [MaxLength(4200)]
        public string MarkdownContent { get; set; } = null!;
        public int OrderIndex { get; set; }
        public int ModuleId { get; set; }
    }
}
