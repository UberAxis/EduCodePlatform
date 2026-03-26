using EduCodePlatform.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;

namespace EduCodePlatform.Infrastructure.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher = new();
        public string Hash(string password)
        {
            return _passwordHasher.HashPassword(null!, password);
        }
        public bool Verify(string passwordHash, string inputPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null!, passwordHash, inputPassword);
            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
