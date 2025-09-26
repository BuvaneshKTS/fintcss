using System;
using System.Threading.Tasks;
using FintcsApi.DTOs;
using FintcsApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FintcsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LedgerController : ControllerBase
    {
        private readonly ILedgerService _ledgerService;

        public LedgerController(ILedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }

        [HttpPost("member/{memberId}/create-ledgers")]
        public async Task<IActionResult> CreateMemberLedgers(Guid memberId)
        {
            await _ledgerService.CreateDefaultLedgersForMemberAsync(memberId);
            return Ok(new { success = true, message = "Ledgers created for member." });
        }

        [HttpPost("transaction")]
        public async Task<IActionResult> RecordTransaction([FromBody] LedgerTransactionDto dto)
        {
            await _ledgerService.RecordTransactionAsync(dto);
            return Ok(new { success = true, message = "Transaction recorded." });
        }
    }
}
