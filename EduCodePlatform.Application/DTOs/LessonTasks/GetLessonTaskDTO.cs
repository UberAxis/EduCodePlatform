namespace EduCodePlatform.Application.DTOs.LessonTasks
{
    public class GetLessonTaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string MarkdownContent { get; set; } = null!;
        public string ExpectedAnswer { get; set; } = null!;
        public int LessonId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
