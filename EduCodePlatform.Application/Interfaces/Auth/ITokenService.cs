using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Application.Interfaces.Auth
{
    public interface ITokenService
    {
        string GenerateJSONWebToken(User user);
    }
}
