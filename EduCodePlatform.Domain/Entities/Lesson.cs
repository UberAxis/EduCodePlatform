using EduCodePlatform.Domain.Common;

namespace EduCodePlatform.Domain.Entities
{
    public class Lesson : BaseEntity
    {
        public string Title { get; protected set; } = null!;
        public string CoverImage { get; protected set; } = null!;
        public string MarkdownContent { get; protected set; } = null!;
        public int OrderIndex { get; protected set; }

        public int ModuleId { get; protected set; }
        public Module Module { get; protected set; } = null!;

        public ICollection<LessonTask> LessonTasks { get; } = new List<LessonTask>();

        protected Lesson() { }

        public Lesson(
            string title,
            string coverImage,
            string markdownContent,
            int orderIndex,
            int moduleId
        )
        {
            Title = title;
            CoverImage = coverImage;
            MarkdownContent = markdownContent;
            OrderIndex = orderIndex;
            ModuleId = moduleId;
        }

        public void UpdateLesson(
            string title,
            string? coverImage,
            string markdownContent,
            int orderIndex,
            int moduleId)
        {
            Title = title;

            if (coverImage != null)
                CoverImage = coverImage;

            MarkdownContent = markdownContent;
            OrderIndex = orderIndex;
            ModuleId = moduleId;
        }
    }
}
