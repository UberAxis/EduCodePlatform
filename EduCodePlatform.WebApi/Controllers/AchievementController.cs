using EduCodePlatform.Application.DTOs.Achievements;
using EduCodePlatform.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduCodePlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IAchievementService _service;

        public AchievementController(IAchievementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAchievementDTO>>> GetAll()
        {
            var achievements = await _service.GetAllAsync();
            return Ok(achievements);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAchievementDTO>> GetById(int id)
        {
            var achievement = await _service.GetByIdAsync(id);
            return Ok(achievement);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetAchievementDTO>> Create(CreateAchievementDTO dto)
        {
            var achievement = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = achievement.Id }, achievement);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetAchievementDTO>> Update(int id, UpdateAchievementDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // Triggers management

        [HttpGet("triggers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetAchievementTriggerDTO>>> GetTriggers()
        {
            var triggers = await _service.GetTriggersAsync();
            return Ok(triggers);
        }

        [HttpGet("triggers/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetAchievementTriggerDTO>> GetTriggerById(int id)
        {
            var trigger = await _service.GetTriggerByIdAsync(id);
            return Ok(trigger);
        }

        [HttpGet("{achievementId}/triggers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetAchievementTriggerDTO>>> GetTriggersByAchievementId(int achievementId)
        {
            var triggers = await _service.GetTriggersByAchievementIdAsync(achievementId);
            return Ok(triggers);
        }

        [HttpPost("triggers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetAchievementTriggerDTO>> CreateTrigger(CreateAchievementTriggerDTO dto)
        {
            var trigger = await _service.CreateTriggerAsync(dto);
            return CreatedAtAction(nameof(GetTriggerById), new { id = trigger.Id }, trigger);
        }

        [HttpPut("triggers/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetAchievementTriggerDTO>> UpdateTrigger(int id, UpdateAchievementTriggerDTO dto)
        {
            var updated = await _service.UpdateTriggerAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("triggers/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTrigger(int id)
        {
            await _service.DeleteTriggerAsync(id);
            return NoContent();
        }

        private Guid? TryGetCurrentUserId()
        {
            var sid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(sid, out var id) ? id : null;
        }
    }
}
