using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FintcsApi.Services.Implementations;

public class MemberService : IMemberService
{
    private readonly AppDbContext _context;
    private readonly ILedgerService _ledgerService;

    public MemberService(AppDbContext context, ILedgerService ledgerService)
    {
        _context = context;
        _ledgerService = ledgerService;
    }

    // Create a new member
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

        await _ledgerService.CreateDefaultLedgersForMemberAsync(member.Id);

        return (true, "Member created successfully", MapToDto(member));
    }

    public async Task<(bool Success, string Message, MemberDto? Data)> GetMemberByIdAsync(Guid memberId)
    {
        var member = await _context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == memberId);
        if (member == null) return (false, "Member not found", null);
        return (true, "Member retrieved", MapToDto(member));
    }

    public async Task<(bool Success, string Message, List<MemberDto> Data)> GetAllMembersBySocietyAsync(Guid societyId)
    {
        var members = await _context.Members.AsNoTracking().Where(m => m.SocietyId == societyId).ToListAsync();
        return (true, "Members retrieved", members.Select(MapToDto).ToList());
    }

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

        await _context.SaveChangesAsync();
        return (true, "Member updated successfully");
    }

    public async Task<(bool Success, string Message)> DeleteMemberAsync(Guid memberId)
    {
        var member = await _context.Members.FindAsync(memberId);
        if (member == null) return (false, "Member not found");

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
        return (true, "Member deleted successfully");
    }

    private MemberDto MapToDto(Member m)
    {
        return new MemberDto
        {
            Id = m.Id,
            SocietyId = m.SocietyId,
            Name = m.Name,
            FHName = m.FHName,
            Mobile = m.Mobile,
            Email = m.Email,
            Status = m.Status,
            OfficeAddress = m.OfficeAddress,
            City = m.City,
            PhoneOffice = m.PhoneOffice,
            Branch = m.Branch,
            PhoneRes = m.PhoneRes,
            Designation = m.Designation,
            ResidenceAddress = m.ResidenceAddress,
            DOB = m.DOB,
            DOJSociety = m.DOJSociety,
            DOR = m.DOR,
            Nominee = m.Nominee,
            NomineeRelation = m.NomineeRelation,
            cdAmount = m.cdAmount,
            Email2 = m.Email2,
            Mobile2 = m.Mobile2,
            Pincode = m.Pincode,
            BankName = m.BankName,
            AccountNumber = m.AccountNumber,
            PayableAt = m.PayableAt,
            Share = m.Share,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt
        };
    }
}
