using EduCodePlatform.Application.DTOs.Users;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EduCodePlatform.WebApi.Controllers
{
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

        [HttpPost("register")]
        public async Task<ActionResult<GetUserDTO>> Register(CreateUserDTO dto)
        {
            var user = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDTO dto)
        {
            var token = await _service.LoginAsync(dto);
            SetJWTCookie(token);
            return Ok(new { token });
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
    }
}
