namespace EduCodePlatform.Application.DTOs.Modules
{
    public class GetModuleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string CoverImage { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int OrderIndex { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
