using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.LessonTasks
{
    public class UpdateLessonTaskDTO
    {
        public string Title { get; set; } = null!;
        [MaxLength(4200)]
        public string MarkdownContent { get; set; } = null!;
        public string ExpectedAnswer { get; set; } = null!;
        public int LessonId { get; set; }
    }
}
