using EduCodePlatform.Application.DTOs.TaskSubmissions;
using EduCodePlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduCodePlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly ITaskSubmissionService _service;

        public SubmissionController(ITaskSubmissionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTaskSubmissionDTO>>> GetAll()
        {
            var tasksubmissions = await _service.GetAllAsync();
            return Ok(tasksubmissions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTaskSubmissionDTO>> GetById(int id)
        {
            var tasksubmission = await _service.GetByIdAsync(id);
            return Ok(tasksubmission);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GetTaskSubmissionDTO>> Create(CreateTaskSubmissionDTO dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

            var tasksubmission = await _service.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = tasksubmission.Id }, tasksubmission);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<GetTaskSubmissionDTO>> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
