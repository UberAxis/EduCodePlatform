using EduCodePlatform.Application.DTOs.Users;
using EduCodePlatform.Application.Interfaces.Services;
using EduCodePlatform.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> GetById(int id)
        {
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
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GetUserDTO>> Update(int id, UpdateUserDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GetUserDTO>> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
