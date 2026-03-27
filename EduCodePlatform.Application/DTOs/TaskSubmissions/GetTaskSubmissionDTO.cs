namespace EduCodePlatform.Application.DTOs.TaskSubmissions
{
    public class GetTaskSubmissionDTO
    {
        public int Id { get; set; }
        public string Answer { get; set; } = null!;
        public bool Result { get; set; }
        public int LessonTaskId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
