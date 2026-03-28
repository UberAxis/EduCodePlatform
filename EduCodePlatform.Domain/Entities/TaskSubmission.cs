using EduCodePlatform.Domain.Common;

namespace EduCodePlatform.Domain.Entities
{
    public class TaskSubmission : BaseEntity
    {
        public string Answer { get; protected set; } = null!;
        public bool Result { get; protected set; }

        public int LessonTaskId { get; protected set; }
        public virtual LessonTask LessonTask { get; protected set; } = null!;

        public Guid UserId { get; protected set; }
        public virtual User User { get; protected set; } = null!;

        protected TaskSubmission() { }

        public TaskSubmission(
            string answer,
            bool result,
            int lessonTaskId,
            Guid userId
        )
        {
            Answer = answer;
            Result = result;
            LessonTaskId = lessonTaskId;
            UserId = userId;
        }
    }
}
