using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoanController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoan([FromBody] LoanCreateUpdateDto dto)
    {
        var (success, message, data) = await _loanService.CreateLoanAsync(dto);
        if (success) return Ok(new { success, message, data });
        return BadRequest(new { success, message });
    }

    [HttpGet("society/{societyId}")]
    public async Task<IActionResult> GetLoansBySociety(Guid societyId)
    {
        var (success, message, data) = await _loanService.GetLoansBySocietyAsync(societyId);
        if (success) return Ok(new { success, message, data });
        return NotFound(new { success, message });
    }

    [HttpGet("{loanId}")]
    public async Task<IActionResult> GetLoan(Guid loanId)
    {
        var (success, message, data) = await _loanService.GetLoanByIdAsync(loanId);
        if (success) return Ok(new { success, message, data });
        return NotFound(new { success, message });
    }

    [HttpPut("{loanId}")]
    public async Task<IActionResult> UpdateLoan(Guid loanId, [FromBody] LoanCreateUpdateDto dto)
    {
        var (success, message) = await _loanService.UpdateLoanAsync(loanId, dto);
        if (success) return Ok(new { success, message });
        return BadRequest(new { success, message });
    }

    [HttpDelete("{loanId}")]
    public async Task<IActionResult> DeleteLoan(Guid loanId)
    {
        var (success, message) = await _loanService.DeleteLoanAsync(loanId);
        if (success) return Ok(new { success, message });
        return BadRequest(new { success, message });
    }

    [HttpGet("member/{memberId}")]
    public async Task<IActionResult> GetLoansByMember(Guid memberId)
    {
        var (success, message, data) = await _loanService.GetLoansByMemberAsync(memberId);
        if (success) return Ok(new { success, message, data });
        return NotFound(new { success, message });
    }

}
