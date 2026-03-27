using EduCodePlatform.Application.Interfaces.Auth;
using EduCodePlatform.Domain.Entities;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace EduCodePlatform.Infrastructure.Auth
{
    public class JwtTokenGenerator : ITokenService
    {
        private readonly JwtOptions _JwtOptions;

        public JwtTokenGenerator(IOptions<JwtOptions> options)
        {
            _JwtOptions = options.Value;
        }

        public string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtOptions.Key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim (ClaimTypes.Name,  user.UserName ?? string.Empty),
                new Claim (ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _JwtOptions.Issuer,
                audience: _JwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_JwtOptions.ExpiresMinutes),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
