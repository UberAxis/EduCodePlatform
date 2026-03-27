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
            try
            {
                var users = await _service.GetAllAsync();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> GetById(Guid id)
        {
            try
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<GetUserDTO>> Register(CreateUserDTO dto)
        {
            try
            {
                var user = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDTO dto)
        {
            try
            {
                var token = await _service.LoginAsync(dto);
                SetJWTCookie(token);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> UpdateProfile(UpdateUserProfileDTO dto)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

                var updated = await _service.UpdateProfileAsync(Guid.Parse(currentUserId), dto);
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("admin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetUserDTO>> AdminUpdateUser(Guid id, AdminUpdateUserDTO dto)
        {
            try
            {
                var updated = await _service.AdminUpdateUserAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
