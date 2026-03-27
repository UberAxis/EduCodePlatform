using EduCodePlatform.Domain.Common;
using EduCodePlatform.Domain.Enums;

namespace EduCodePlatform.Domain.Entities
{
    public class User : BaseEntity
    {

        public string Name { get; protected set; } = null!;
        public string HashPassword { get; protected set; } = null!;
        public string Avatar { get; protected set; } = null!;
        public UserRole Role { get; protected set; }

        protected User() { }

        public User(
            string name,
            string hashPassword,
            string avatar,
            UserRole role = UserRole.User)
        {
            Name = name;
            HashPassword = hashPassword;
            Avatar = avatar;
            Role = role;
        }

        public void UpdateUser(
            string name,
            string? avatar)
        {
            Name = name;

            if (avatar != null)
                Avatar = Avatar;
        }

        public void ChangePassword(string hash)
        {
            HashPassword = hash;
        }
    }
}
