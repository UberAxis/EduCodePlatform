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

        // код привязки (6 цифр, expires через 10 минут)
        public string? LinkCode { get; set; }
        public DateTime? LinkCodeExpiresAt { get; set; }

        public virtual ICollection<TaskSubmission> Submissions { get; set; } = new List<TaskSubmission>();
        public virtual ICollection<UserAchievement> Achievements { get; set; } = new List<UserAchievement>();
        public virtual ICollection<UserLessonProgress> LessonProgress { get; set; } = new List<UserLessonProgress>();

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public User() { }

        public User(string userName, UserRole role = UserRole.User)
        {
            UserName = userName;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateUser(string userName)
        {
            UserName = userName;
        }
    }
}
