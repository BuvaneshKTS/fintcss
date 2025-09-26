using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SocietyController : ControllerBase
{
    private readonly ISocietyService _societyService;

    public SocietyController(ISocietyService societyService)
    {
        _societyService = societyService;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateSociety([FromBody] SocietyCreateUpdateDto societyDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _societyService.CreateSocietyAsync(societyDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSociety(Guid id)
    {
        var result = await _societyService.GetSocietyByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSocieties()
    {
        var result = await _societyService.GetAllSocietiesAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateSociety(Guid id, [FromBody] SocietyCreateUpdateDto societyDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _societyService.UpdateSocietyAsync(id, societyDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteSociety(Guid id)
    {
        var result = await _societyService.DeleteSocietyAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
