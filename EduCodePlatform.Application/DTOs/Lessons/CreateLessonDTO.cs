using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.Lessons
{
    public class CreateLessonDTO
    {
        [Required]
        public string Title { get; set; } = null!;
        public string? CoverImage { get; set; }
        [Required]
        [MaxLength(4200)]
        public string MarkdownContent { get; set; } = null!;
        [Required]
        public int OrderIndex { get; set; }
        [Required]
        public int ModuleId { get; set; }
    }
}
