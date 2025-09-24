using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FintcsApi.Services.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly AppDbContext _context;

        public MemberService(AppDbContext context)
        {
            _context = context;
        }

        // Create a member
        public async Task<(bool Success, string Message, MemberDto? Data)> CreateMemberAsync(Guid societyId, MemberCreateUpdateDto dto)
        {
            var society = await _context.Societies.FindAsync(societyId);
            if (society == null) return (false, "Society not found", null);

            var member = new Member
            {
                SocietyId = societyId,
                Name = dto.Name,
                FHName = dto.FHName,
                Mobile = dto.Mobile,
                Email = dto.Email,
                Status = dto.Status,
                OfficeAddress = dto.OfficeAddress,
                City = dto.City,
                PhoneOffice = dto.PhoneOffice,
                Branch = dto.Branch,
                PhoneRes = dto.PhoneRes,
                Designation = dto.Designation,
                ResidenceAddress = dto.ResidenceAddress,
                DOB = dto.DOB,
                DOJSociety = dto.DOJSociety,
                DOR = dto.DOR,
                Nominee = dto.Nominee,
                NomineeRelation = dto.NomineeRelation,
                cdAmount = dto.cdAmount,
                Email2 = dto.Email2,
                Mobile2 = dto.Mobile2,
                Pincode = dto.Pincode,
                BankName = dto.BankName,
                AccountNumber = dto.AccountNumber,
                PayableAt = dto.PayableAt,
                Share = dto.Share,
                CreatedAt = DateTime.UtcNow
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return (true, "Member created successfully", MapToDto(member));
        }

        // Get a member by its Id
        public async Task<(bool Success, string Message, MemberDto? Data)> GetMemberByIdAsync(Guid memberId)
        {
            // Use FirstOrDefaultAsync to ensure EF can find the member
            var member = await _context.Members
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == memberId);

            if (member == null) return (false, "Member not found", null);
            return (true, "Member found", MapToDto(member));
        }

        // Get all members of a society
        public async Task<(bool Success, string Message, List<MemberDto> Data)> GetAllMembersBySocietyAsync(Guid societyId)
        {
            var members = await _context.Members
                .AsNoTracking()
                .Where(m => m.SocietyId == societyId)
                .ToListAsync();

            var memberDtos = members.Select(MapToDto).ToList();
            return (true, "Members retrieved successfully", memberDtos);
        }

        // Update member
        public async Task<(bool Success, string Message)> UpdateMemberAsync(Guid memberId, MemberCreateUpdateDto dto)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null) return (false, "Member not found");

            member.Name = dto.Name;
            member.FHName = dto.FHName;
            member.Mobile = dto.Mobile;
            member.Email = dto.Email;
            member.Status = dto.Status;
            member.OfficeAddress = dto.OfficeAddress;
            member.City = dto.City;
            member.PhoneOffice = dto.PhoneOffice;
            member.Branch = dto.Branch;
            member.PhoneRes = dto.PhoneRes;
            member.Designation = dto.Designation;
            member.ResidenceAddress = dto.ResidenceAddress;
            member.DOB = dto.DOB;
            member.DOJSociety = dto.DOJSociety;
            member.DOR = dto.DOR;
            member.Nominee = dto.Nominee;
            member.NomineeRelation = dto.NomineeRelation;
            member.cdAmount = dto.cdAmount;
            member.Email2 = dto.Email2;
            member.Mobile2 = dto.Mobile2;
            member.Pincode = dto.Pincode;
            member.BankName = dto.BankName;
            member.AccountNumber = dto.AccountNumber;
            member.PayableAt = dto.PayableAt;
            member.Share = dto.Share;
            member.UpdatedAt = DateTime.UtcNow;

            _context.Members.Update(member);
            await _context.SaveChangesAsync();

            return (true, "Member updated successfully");
        }

        // Delete member
        public async Task<(bool Success, string Message)> DeleteMemberAsync(Guid memberId)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null) return (false, "Member not found");

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return (true, "Member deleted successfully");
        }

        // Map Member → MemberDto
        private MemberDto MapToDto(Member member)
        {
            return new MemberDto
            {
                Id = member.Id,
                SocietyId = member.SocietyId,
                Name = member.Name,
                FHName = member.FHName,
                Mobile = member.Mobile,
                Email = member.Email,
                Status = member.Status,
                OfficeAddress = member.OfficeAddress,
                City = member.City,
                PhoneOffice = member.PhoneOffice,
                Branch = member.Branch,
                PhoneRes = member.PhoneRes,
                Designation = member.Designation,
                ResidenceAddress = member.ResidenceAddress,
                DOB = member.DOB,
                DOJSociety = member.DOJSociety,
                DOR = member.DOR,
                Nominee = member.Nominee,
                NomineeRelation = member.NomineeRelation,
                cdAmount = member.cdAmount,
                Email2 = member.Email2,
                Mobile2 = member.Mobile2,
                Pincode = member.Pincode,
                BankName = member.BankName,
                AccountNumber = member.AccountNumber,
                PayableAt = member.PayableAt,
                Share = member.Share,
                CreatedAt = member.CreatedAt,
                UpdatedAt = member.UpdatedAt
            };
        }
    }
}
