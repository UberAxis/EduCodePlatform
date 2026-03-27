using EduCodePlatform.Application.DTOs.LessonTasks;
using EduCodePlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduCodePlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ILessonTaskService _service;

        public TaskController(ILessonTaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetLessonTaskDTO>>> GetAll()
        {
            var lessontasks = await _service.GetAllAsync();
            return Ok(lessontasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetLessonTaskDTO>> GetById(int id)
        {
            var lessontask = await _service.GetByIdAsync(id);
            return Ok(lessontask);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetLessonTaskDTO>> Create(CreateLessonTaskDTO dto)
        {
            var lessontask = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = lessontask.Id }, lessontask);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetLessonTaskDTO>> Update(int id, UpdateLessonTaskDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetLessonTaskDTO>> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
