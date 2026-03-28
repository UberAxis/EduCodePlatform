using EduCodePlatform.Domain.Common;

namespace EduCodePlatform.Domain.Entities
{
    public class UserLessonProgress : BaseEntity
    {
        public Guid UserId { get; protected set; }
        public virtual User User { get; protected set; } = null!;

        public int LessonId { get; protected set; }
        public virtual Lesson Lesson { get; protected set; } = null!;

        public bool IsCompleted { get; protected set; }
        public DateTime? CompletedAt { get; protected set; }

        protected UserLessonProgress() { }

        public UserLessonProgress(Guid userId, int lessonId, bool isCompleted = false)
        {
            UserId = userId;
            LessonId = lessonId;
            IsCompleted = isCompleted;
            if (isCompleted) CompletedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }
    }
}
