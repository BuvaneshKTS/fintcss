using FintcsApi.DTOs;

using FintcsApi.Models;

namespace FintcsApi.Services.Interfaces
{
    public interface ILoanService
    {
        Task<(bool Success, string Message, LoanDto? Data)> CreateLoanAsync(LoanCreateUpdateDto dto);
        Task<(bool Success, string Message, IEnumerable<LoanDto>? Data)> GetLoansBySocietyAsync(Guid societyId);
        Task<(bool Success, string Message, LoanDto? Data)> GetLoanByIdAsync(Guid loanId);
        Task<(bool Success, string Message)> UpdateLoanAsync(Guid loanId, LoanCreateUpdateDto dto);
        Task<(bool Success, string Message)> DeleteLoanAsync(Guid loanId);
        Task<(bool Success, string Message, IEnumerable<LoanDto>? Data)> GetLoansByMemberAsync(Guid memberId);

    }
}
