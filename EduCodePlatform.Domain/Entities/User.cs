using EduCodePlatform.Domain.Common;
using EduCodePlatform.Domain.Enums;

namespace EduCodePlatform.Domain.Entities
{
    public class User : BaseEntity
    {

        public string Name { get; protected set; } = null!;
        public string HashPassword { get; protected set; } = null!;
        public UserRole Role { get; protected set; }

        protected User() { }

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

        public void ChangePassword(string hash)
        {
            HashPassword = hash;
        }
    }
}
