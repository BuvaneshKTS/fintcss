// File: Services/Interfaces/ILoanTypeService.cs
using FintcsApi.DTOs;
using FintcsApi.Models;

namespace FintcsApi.Services.Interfaces;

public interface ILoanTypeService
{
    Task<ApiResponse<LoanTypeDto>> CreateLoanTypeAsync(LoanTypeDto dto);
    Task<ApiResponse<LoanTypeDto>> GetLoanTypeByIdAsync(Guid id);
    Task<ApiResponse<List<LoanTypeDto>>> GetAllLoanTypesBySocietyAsync(Guid societyId);
    Task<ApiResponse<LoanTypeDto>> UpdateLoanTypeAsync(Guid id, LoanTypeDto dto);
    Task<ApiResponse<bool>> DeleteLoanTypeAsync(Guid id);
}

