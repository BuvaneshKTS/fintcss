using FintcsApi.DTOs;
using FintcsApi.Models;

namespace FintcsApi.Services.Interfaces
{
    public interface ISocietyService
    {
        Task<ApiResponse<SocietyDto>> CreateSocietyAsync(SocietyCreateUpdateDto dto);
        Task<ApiResponse<SocietyDto>> GetSocietyByIdAsync(Guid id);
        Task<ApiResponse<List<SocietyDto>>> GetAllSocietiesAsync();
        Task<ApiResponse<SocietyDto>> UpdateSocietyAsync(Guid id, SocietyCreateUpdateDto dto);
        Task<ApiResponse<bool>> DeleteSocietyAsync(Guid id);
    }
}
