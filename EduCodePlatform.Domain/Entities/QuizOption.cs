using EduCodePlatform.Domain.Common;

namespace EduCodePlatform.Domain.Entities
{
    public class QuizOption : BaseEntity
    {
        public string Text { get; protected set; } = null!;

        public int LessonTaskId { get; protected set; }
        public LessonTask LessonTask { get; protected set; } = null!;

        protected QuizOption() { }

        public QuizOption(string text)
        {
            Text = text;
        }
    }
}
