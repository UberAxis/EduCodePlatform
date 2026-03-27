using EduCodePlatform.Domain.Common;

namespace EduCodePlatform.Domain.Entities
{
    public class TaskSubmission : BaseEntity
    {
        public string Answer { get; protected set; } = null!;
        public bool Result { get; protected set; }

        public int LessonTaskId { get; protected set; }
        public LessonTask LessonTask { get; protected set; } = null!;

        protected TaskSubmission() { }

        public TaskSubmission(
            string answer,
            bool result,
            int lessonTaskId
        )
        {
            Answer = answer;
            Result = result;
            LessonTaskId = lessonTaskId;
        }
    }
}
