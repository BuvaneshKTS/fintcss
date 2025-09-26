// File: Services/Interfaces/IBankAccountService.cs
using FintcsApi.DTOs;
using FintcsApi.Models;


namespace FintcsApi.Services.Interfaces;

public interface IBankAccountService
{
    Task<ApiResponse<BankAccountDto>> CreateBankAccountAsync(BankAccountDto dto);
    Task<ApiResponse<BankAccountDto>> GetBankAccountByIdAsync(Guid id);
    Task<ApiResponse<List<BankAccountDto>>> GetAllBankAccountsBySocietyAsync(Guid societyId);
    Task<ApiResponse<BankAccountDto>> UpdateBankAccountAsync(Guid id, BankAccountDto dto);
    Task<ApiResponse<bool>> DeleteBankAccountAsync(Guid id);
}
