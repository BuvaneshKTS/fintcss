// File: FintcsApi/Services/Interfaces/ISocietyService.cs
using FintcsApi.DTOs;
using FintcsApi.Models;

namespace FintcsApi.Services.Interfaces;

public interface ISocietyService
{
    Task<ApiResponse<Society>> CreateSocietyAsync(SocietyCreateUpdateDto societyDto);
    Task<ApiResponse<Society>> GetSocietyByIdAsync(Guid id);
    Task<ApiResponse<List<Society>>> GetAllSocietiesAsync();
    Task<ApiResponse<Society>> UpdateSocietyAsync(Guid id, SocietyCreateUpdateDto societyDto);
    Task<ApiResponse<bool>> DeleteSocietyAsync(Guid id);
}