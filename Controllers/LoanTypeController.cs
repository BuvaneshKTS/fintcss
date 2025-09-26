using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoanTypeController : ControllerBase
{
    private readonly ILoanTypeService _loanTypeService;

    public LoanTypeController(ILoanTypeService loanTypeService)
    {
        _loanTypeService = loanTypeService;
    }

    // POST: api/loantype
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateLoanType([FromBody] LoanTypeDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _loanTypeService.CreateLoanTypeAsync(dto);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    // GET: api/loantype/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanType(Guid id)
    {
        var result = await _loanTypeService.GetLoanTypeByIdAsync(id);

        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }

    // GET: api/loantype/society/{societyId}
    [HttpGet("society/{societyId}")]
    public async Task<IActionResult> GetAllLoanTypesBySociety(Guid societyId)
    {
        var result = await _loanTypeService.GetAllLoanTypesBySocietyAsync(societyId);

        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }

    // PUT: api/loantype/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateLoanType(Guid id, [FromBody] LoanTypeDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _loanTypeService.UpdateLoanTypeAsync(id, dto);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    // DELETE: api/loantype/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteLoanType(Guid id)
    {
        var result = await _loanTypeService.DeleteLoanTypeAsync(id);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}
