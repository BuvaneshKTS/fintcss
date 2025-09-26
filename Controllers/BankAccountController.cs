using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BankAccountController : ControllerBase
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }

    // POST: api/bankaccount
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateBankAccount([FromBody] BankAccountDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bankAccountService.CreateBankAccountAsync(dto);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    // GET: api/bankaccount/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBankAccount(Guid id)
    {
        var result = await _bankAccountService.GetBankAccountByIdAsync(id);

        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }

    // GET: api/bankaccount/society/{societyId}
    [HttpGet("society/{societyId}")]
    public async Task<IActionResult> GetAllBankAccountsBySociety(Guid societyId)
    {
        var result = await _bankAccountService.GetAllBankAccountsBySocietyAsync(societyId);

        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }

    // PUT: api/bankaccount/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateBankAccount(Guid id, [FromBody] BankAccountDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bankAccountService.UpdateBankAccountAsync(id, dto);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    // DELETE: api/bankaccount/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteBankAccount(Guid id)
    {
        var result = await _bankAccountService.DeleteBankAccountAsync(id);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}
