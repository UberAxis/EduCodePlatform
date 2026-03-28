namespace EduCodePlatform.Application.DTOs.LessonTasks
{
    public class GetLessonTaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string MarkdownContent { get; set; } = null!;
        public string ExpectedAnswer { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? InitialCode { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public List<QuizOptionDTO> QuizOptions { get; set; } = new();
        public int LessonId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
