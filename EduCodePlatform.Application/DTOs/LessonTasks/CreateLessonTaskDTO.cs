using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.LessonTasks
{
    public class CreateLessonTaskDTO
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        [MaxLength(4200)]
        public string MarkdownContent { get; set; } = null!;
        [Required]
        public string ExpectedAnswer { get; set; } = null!;
        [Required]
        public int LessonId { get; set; }
    }
}
