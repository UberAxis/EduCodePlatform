namespace EduCodePlatform.Application.DTOs.Users
{
    public class GetUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
