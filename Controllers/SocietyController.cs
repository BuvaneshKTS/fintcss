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
    private readonly IMemberService _memberService;

    public SocietyController(ISocietyService societyService, IMemberService memberService)
    {
        _societyService = societyService;
        _memberService = memberService;
    }


    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateSociety([FromBody] SocietyCreateUpdateDto societyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _societyService.CreateSocietyAsync(societyDto);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSociety(Guid id)
    {
        var result = await _societyService.GetSocietyByIdAsync(id);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSocieties()
    {
        var result = await _societyService.GetAllSocietiesAsync();
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateSociety(Guid id, [FromBody] SocietyCreateUpdateDto societyDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _societyService.UpdateSocietyAsync(id, societyDto);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteSociety(Guid id)
    {
        var result = await _societyService.DeleteSocietyAsync(id);
        
        if (result.Success)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    // ----------------------
    // Create Member
    // POST: /api/society/{societyId}/members
    // ----------------------
    [HttpPost("{societyId}/members")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateMember(Guid societyId, [FromBody] MemberCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _memberService.CreateMemberAsync(societyId, dto);
        if (result.Success) return Ok(result);
        return BadRequest(result);
    }

    // ----------------------
    // Get All Members of a Society
    // GET: /api/society/{societyId}/members
    // ----------------------
    [HttpGet("{societyId}/members")]
    public async Task<IActionResult> GetAllMembers(Guid societyId)
    {
        var result = await _memberService.GetAllMembersBySocietyAsync(societyId);
        if (result.Success) return Ok(result);
        return NotFound(result);
    }

    // ----------------------
    // Get Member by Id
    // GET: /api/society/members/{memberId}
    // ----------------------
    [HttpGet("members/{memberId}")]
    public async Task<IActionResult> GetMember(Guid memberId)
    {
        var result = await _memberService.GetMemberByIdAsync(memberId);
        if (result.Success) return Ok(result);
        return NotFound(result);
    }

    // ----------------------
    // Update Member
    // PUT: /api/society/members/{memberId}
    // ----------------------
    [HttpPut("members/{memberId}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateMember(Guid memberId, [FromBody] MemberCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _memberService.UpdateMemberAsync(memberId, dto);
        if (result.Success) return Ok(result);
        return BadRequest(result);
    }

    // ----------------------
    // Delete Member
    // DELETE: /api/society/members/{memberId}
    // ----------------------
    [HttpDelete("members/{memberId}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteMember(Guid memberId)
    {
        var result = await _memberService.DeleteMemberAsync(memberId);
        if (result.Success) return Ok(result);
        return BadRequest(result);
    }


    

}