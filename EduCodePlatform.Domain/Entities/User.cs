using EduCodePlatform.Domain.Common;
using EduCodePlatform.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace EduCodePlatform.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }
        
        // геймификация (ну и слово лол)
        public int ExperiencePoints { get; set; } = 0;
        public int Level { get; set; } = 1;
        public int Coins { get; set; } = 0;
        
        // статистика
        public int TasksCompletedCount { get; set; } = 0;
        public int QuizzesPassedCount { get; set; } = 0;

        // род.контроль
        public Guid? ParentId { get; set; }
        public virtual User? Parent { get; set; }
        public virtual ICollection<User> Children { get; set; } = new List<User>();

        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public User() { }

        public User(
            string name,
            string hashPassword,
            UserRole role = UserRole.User)
        {
            Name = name;
            HashPassword = hashPassword;
            Role = role;
        }

        public void UpdateUser(string name)
        {
            Name = name;
        }

        public void UpdateUser(string userName)
        {
            UserName = userName;
        }
    }
}
