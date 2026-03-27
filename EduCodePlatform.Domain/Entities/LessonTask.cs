using EduCodePlatform.Domain.Common;

namespace EduCodePlatform.Domain.Entities
{
    public class LessonTask : BaseEntity
    {
        public string Title { get; protected set; } = null!;
        public string MarkdownContent { get; protected set; } = null!;
        public string ExpectedAnswer { get; protected set; } = null!;

        public int LessonId { get; protected set; }
        public Lesson Lesson { get; protected set; } = null!;

        public ICollection<TaskSubmission> TaskSubmissions { get; } = new List<TaskSubmission>();

        protected LessonTask() { }

        public LessonTask(
            string title,
            string markdownContent,
            string expectedAnswer,
            int lessonId
        )
        {
            Title = title;
            MarkdownContent = markdownContent;
            ExpectedAnswer = expectedAnswer;
            LessonId = lessonId;
        }

        public void UpdateLessonTask(
            string title,
            string markdownContent,
            string expectedAnswer,
            int lessonId)
        {
            Title = title;
            MarkdownContent = markdownContent;
            ExpectedAnswer = expectedAnswer;
            LessonId = lessonId;
        }
    }
}
