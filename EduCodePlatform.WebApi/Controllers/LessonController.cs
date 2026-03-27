using EduCodePlatform.Application.DTOs.Lessons;
using EduCodePlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCodePlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _service;

        public LessonController(ILessonService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetLessonDTO>>> GetAll()
        {
            var lessons = await _service.GetAllAsync();
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetLessonDTO>> GetById(int id)
        {
            var lesson = await _service.GetByIdAsync(id);
            return Ok(lesson);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetLessonDTO>> Create(CreateLessonDTO dto)
        {
            var lesson = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = lesson.Id }, lesson);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetLessonDTO>> Update(int id, UpdateLessonDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetLessonDTO>> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
