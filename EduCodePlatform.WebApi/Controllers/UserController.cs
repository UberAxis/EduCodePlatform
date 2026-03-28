using EduCodePlatform.Application.DTOs.Users;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EduCodePlatform.WebApi.Controllers
{
    public record LinkChildByCodeRequest(string Code);

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly JwtOptions _jwtOptions;

        public UserController(IUserService service, IOptions<JwtOptions> jwtOptions)
        {
            _service = service;
            _jwtOptions = jwtOptions.Value;
        }

        private void SetJWTCookie(string token)
        {
            var CookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresMinutes),
                Secure = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("jwt_token", token, CookieOptions);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> GetById(Guid id)
        {
            // юзер может видеть только себя, если он не админ
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && currentUserId != id.ToString())
            {
                return Forbid();
            }

            var user = await _service.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> GetMe()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

            var user = await _service.GetMeAsync(userId);
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<GetUserDTO>> Register(CreateUserDTO dto)
        {
            var user = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginUserDTO dto)
        {
            var response = await _service.LoginAsync(dto);
            SetJWTCookie(response.Token);
            return Ok(response);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> UpdateProfile(UpdateUserProfileDTO dto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            var updated = await _service.UpdateProfileAsync(Guid.Parse(currentUserId), dto);
            return Ok(updated);
        }

        [HttpPut("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetUserDTO>> AdminUpdateUser(Guid id, AdminUpdateUserDTO dto)
        {
            var updated = await _service.AdminUpdateUserAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("link-child")]
        [Authorize]
        public async Task<ActionResult> LinkChild(string childUserName)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

            await _service.LinkChildAsync(userId, childUserName);
            return Ok(new { message = "Ребенок успешно привязан" });
        }

        [HttpGet("my-children")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetMyChildren()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

            var children = await _service.GetChildrenAsync(userId);
            return Ok(children);
        }

        [HttpPost("generate-link-code")]
        [Authorize]
        public async Task<ActionResult> GenerateLinkCode()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

            var code = await _service.GenerateLinkCodeAsync(userId);
            return Ok(new { code, expiresInMinutes = 10 });
        }

        [HttpPost("link-child-by-code")]
        [Authorize]
        public async Task<ActionResult> LinkChildByCode([FromBody] LinkChildByCodeRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

            await _service.LinkChildByCodeAsync(userId, request.Code);
            return Ok(new { message = "Ребёнок успешно привязан" });
        }

        [HttpDelete("unlink-child/{childId}")]
        [Authorize]
        public async Task<ActionResult> UnlinkChild(Guid childId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

            await _service.UnlinkChildAsync(userId, childId);
            return Ok(new { message = "Ребёнок отвязан" });
        }

        [HttpGet("leaderboard")]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetLeaderboard(int count = 10)
        {
            var top = await _service.GetLeaderboardAsync(count);
            return Ok(top);
        }
    }
}
