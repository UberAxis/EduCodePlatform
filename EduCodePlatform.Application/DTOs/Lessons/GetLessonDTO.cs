namespace EduCodePlatform.Application.DTOs.Lessons
{
    public class GetLessonDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string CoverImage { get; set; } = null!;
        public string MarkdownContent { get; set; } = null!;
        public int OrderIndex { get; set; }
        public int ModuleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
