using EduCodePlatform.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.LessonTasks
{
    public class UpdateLessonTaskDTO
    {
        public string Title { get; set; } = null!;
        [MaxLength(4200)]
        public string MarkdownContent { get; set; } = null!;
        public string ExpectedAnswer { get; set; } = null!;
        
        public TaskType Type { get; set; } = TaskType.Quiz;
        public string? InitialCode { get; set; }
        public int CorrectAnswerIndex { get; set; } = 0;
        public List<QuizOptionDTO>? QuizOptions { get; set; }

        public int LessonId { get; set; }
    }
}
