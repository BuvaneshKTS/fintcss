// Services/Interfaces/ILedgerService.cs
using FintcsApi.DTOs;
using System;
using System.Threading.Tasks;

using FintcsApi.Models;

namespace FintcsApi.Services.Interfaces
{
    public interface ILedgerService
    {
        Task RecordTransactionAsync(LedgerTransactionDto dto);
        Task CreateDefaultLedgersForMemberAsync(Guid memberId);
        Task CreateOtherLedgerAsync(Guid? memberId, string accountName, decimal initialBalance = 0);
        Task RecordOtherLedgerTransactionAsync(LedgerTransactionDto dto);
    }
}
