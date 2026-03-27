using EduCodePlatform.Domain.Common;

namespace EduCodePlatform.Domain.Entities
{
    public class Module : BaseEntity
    {
        public string Title { get; protected set; } = null!;
        public string CoverImage { get; protected set; } = null!;
        public string Description { get; protected set; } = null!;
        public int OrderIndex { get; protected set; }

        public ICollection<Lesson> Lessons { get; } = new List<Lesson>();

        protected Module() { }

        public Module(
            string title,
            string coverImage,
            string description,
            int orderIndex
        )
        {
            Title = title;
            CoverImage = coverImage;
            Description = description;
            OrderIndex = orderIndex;
        }

        public void UpdateModule(
            string title,
            string? coverImage,
            string description,
            int orderIndex)
        {
            Title = title;
            
            if (coverImage != null)
                CoverImage = coverImage;

            Description = description;
            OrderIndex = orderIndex;
        }
    }
}
