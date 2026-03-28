using EduCodePlatform.Domain.Common;
using EduCodePlatform.Domain.Enums;

namespace EduCodePlatform.Domain.Entities
{
    public class LessonTask : BaseEntity
    {
        public string Title { get; protected set; } = null!;
        public string MarkdownContent { get; protected set; } = null!;
        public string ExpectedAnswer { get; protected set; } = null!;
        
        public TaskType Type { get; protected set; } = TaskType.Quiz;
        public string? InitialCode { get; protected set; }
        public int CorrectAnswerIndex { get; protected set; } = 0;

        public int LessonId { get; protected set; }
        public Lesson Lesson { get; protected set; } = null!;

        public ICollection<TaskSubmission> TaskSubmissions { get; } = new List<TaskSubmission>();
        public ICollection<QuizOption> QuizOptions { get; } = new List<QuizOption>();

        protected LessonTask() { }

        public LessonTask(
            string title,
            string markdownContent,
            string expectedAnswer,
            int lessonId,
            TaskType type = TaskType.Quiz,
            string? initialCode = null,
            int correctAnswerIndex = 0
        )
        {
            Title = title;
            MarkdownContent = markdownContent;
            ExpectedAnswer = expectedAnswer;
            LessonId = lessonId;
            Type = type;
            InitialCode = initialCode;
            CorrectAnswerIndex = correctAnswerIndex;
        }

        public void UpdateLessonTask(
            string title,
            string markdownContent,
            string expectedAnswer,
            int lessonId,
            TaskType type,
            string? initialCode,
            int correctAnswerIndex = 0)
        {
            Title = title;
            MarkdownContent = markdownContent;
            ExpectedAnswer = expectedAnswer;
            LessonId = lessonId;
            Type = type;
            InitialCode = initialCode;
            CorrectAnswerIndex = correctAnswerIndex;
        }
    }
}
