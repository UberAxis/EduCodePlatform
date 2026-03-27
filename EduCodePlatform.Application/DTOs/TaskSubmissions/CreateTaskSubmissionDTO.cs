using System.ComponentModel.DataAnnotations;

namespace EduCodePlatform.Application.DTOs.TaskSubmissions
{
    public class CreateTaskSubmissionDTO
    {
        [Required]
        public string Answer { get; set; } = null!;
        [Required]
        public int LessonTaskId { get; set; }
    }
}
