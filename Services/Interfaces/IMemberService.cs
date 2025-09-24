// File: Services/Interfaces/IMemberService.cs
using FintcsApi.DTOs;

namespace FintcsApi.Services.Interfaces
{
    public interface IMemberService
    {
        // Create a new member for a specific society
        Task<(bool Success, string Message, MemberDto? Data)> CreateMemberAsync(Guid societyId, MemberCreateUpdateDto dto);

        // Get a member by its Id
        Task<(bool Success, string Message, MemberDto? Data)> GetMemberByIdAsync(Guid memberId);

        // Get all members of a specific society
        Task<(bool Success, string Message, List<MemberDto> Data)> GetAllMembersBySocietyAsync(Guid societyId);

        // Update an existing member by Id
        Task<(bool Success, string Message)> UpdateMemberAsync(Guid memberId, MemberCreateUpdateDto dto);

        // Delete a member by Id
        Task<(bool Success, string Message)> DeleteMemberAsync(Guid memberId);
    }
}
