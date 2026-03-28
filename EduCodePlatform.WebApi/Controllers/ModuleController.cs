using EduCodePlatform.Application.DTOs.Modules;
using EduCodePlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduCodePlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _service;

        public ModuleController(IModuleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetModuleDTO>>> GetAll()
        {
            var modules = await _service.GetAllAsync(TryGetCurrentUserId());
            return Ok(modules);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetModuleDTO>> GetById(int id)
        {
            var module = await _service.GetByIdAsync(id, TryGetCurrentUserId());
            return Ok(module);
        }

        private Guid? TryGetCurrentUserId()
        {
            var sid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(sid, out var id) ? id : null;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetModuleDTO>> Create(CreateModuleDTO dto)
        {
            var module = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = module.Id }, module);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetModuleDTO>> Update(int id, UpdateModuleDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetModuleDTO>> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
