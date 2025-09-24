// File: FintcsApi/Services/Implementations/SocietyService.cs
using Microsoft.EntityFrameworkCore;
using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;

namespace FintcsApi.Services.Implementations;

public class SocietyService : ISocietyService
{
    private readonly AppDbContext _context;

    public SocietyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<Society>> CreateSocietyAsync(SocietyCreateUpdateDto dto)
    {
        try
        {
            // Check uniqueness
            if (!await IsSocietyUniqueAsync(dto))
                return ApiResponse<Society>.ErrorResponse("Society with same Name, Email, Phone, Fax, Address, Website or RegistrationNumber already exists.");

            if (!AreLoanTypesUnique(dto.LoanTypes))
                return ApiResponse<Society>.ErrorResponse("Duplicate LoanType names are not allowed within the society.");

            if (!AreBankAccountsUnique(dto.BankAccounts))
                return ApiResponse<Society>.ErrorResponse("Duplicate BankName + AccountNumber are not allowed within the society.");

            var society = new Society
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                Phone = dto.Phone,
                Fax = dto.Fax,
                Email = dto.Email,
                Website = dto.Website,
                RegistrationNumber = dto.RegistrationNumber,
                ChequeBounceCharge = dto.ChequeBounceCharge,
                CreatedAt = DateTime.UtcNow
            };

            // Add LoanTypes
            foreach (var lt in dto.LoanTypes)
            {
                society.LoanTypes.Add(new LoanType
                {
                    Name = lt.Name,
                    InterestPercent = lt.InterestPercent,
                    LimitAmount = lt.LimitAmount,
                    CompulsoryDeposit = lt.CompulsoryDeposit,
                    OptionalDeposit = lt.OptionalDeposit,
                    ShareAmount = lt.ShareAmount,
                    XTimes = lt.XTimes,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Add BankAccounts
            foreach (var ba in dto.BankAccounts)
            {
                society.BankAccounts.Add(new SocietyBankAccount
                {
                    BankName = ba.BankName,
                    AccountNumber = ba.AccountNumber,
                    IFSC = ba.IFSC,
                    Branch = ba.Branch,
                    Notes = ba.Notes,
                    IsPrimary = ba.IsPrimary,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Add Members
            if (dto.Members != null && dto.Members.Any())
            {
                foreach (var m in dto.Members)
                {
                    society.Members.Add(new Member
                    {
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
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            _context.Societies.Add(society);
            await _context.SaveChangesAsync();

            var created = await _context.Societies
                .Include(s => s.LoanTypes)
                .Include(s => s.BankAccounts)
                .Include(s => s.Members) // ✅ Include members
                .FirstAsync(s => s.Id == society.Id);

            return ApiResponse<Society>.SuccessResponse(created, "Society created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Society>.ErrorResponse($"Error creating society: {ex.Message}");
        }
    }

    public async Task<ApiResponse<Society>> UpdateSocietyAsync(Guid id, SocietyCreateUpdateDto dto)
    {
        try
        {
            var society = await _context.Societies
                .Include(s => s.LoanTypes)
                .Include(s => s.BankAccounts)
                .Include(s => s.Members) // ✅ Include members
                .FirstOrDefaultAsync(s => s.Id == id);

            if (society == null) return ApiResponse<Society>.ErrorResponse("Society not found.");

            if (!await IsSocietyUniqueAsync(dto, id))
                return ApiResponse<Society>.ErrorResponse("Society with same Name, Email, Phone, Fax, Address, Website or RegistrationNumber already exists.");

            if (!AreLoanTypesUnique(dto.LoanTypes))
                return ApiResponse<Society>.ErrorResponse("Duplicate LoanType names are not allowed within the society.");

            if (!AreBankAccountsUnique(dto.BankAccounts))
                return ApiResponse<Society>.ErrorResponse("Duplicate BankName + AccountNumber are not allowed within the society.");

            // Update Society
            society.Name = dto.Name;
            society.Address = dto.Address;
            society.City = dto.City;
            society.Phone = dto.Phone;
            society.Fax = dto.Fax;
            society.Email = dto.Email;
            society.Website = dto.Website;
            society.RegistrationNumber = dto.RegistrationNumber;
            society.ChequeBounceCharge = dto.ChequeBounceCharge;
            society.UpdatedAt = DateTime.UtcNow;

            UpdateLoanTypes(society, dto.LoanTypes);
            UpdateBankAccounts(society, dto.BankAccounts);
            UpdateMembers(society, dto.Members);

            await _context.SaveChangesAsync();

            var updated = await _context.Societies
                .Include(s => s.LoanTypes)
                .Include(s => s.BankAccounts)
                .Include(s => s.Members) // ✅ Include members
                .FirstAsync(s => s.Id == society.Id);

            return ApiResponse<Society>.SuccessResponse(updated, "Society updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Society>.ErrorResponse($"Error updating society: {ex.Message}");
        }
    }

    public async Task<ApiResponse<Society>> GetSocietyByIdAsync(Guid id)
    {
        var society = await _context.Societies
            .Include(s => s.LoanTypes)
            .Include(s => s.BankAccounts)
            .Include(s => s.Members) // ✅ Include members
            .FirstOrDefaultAsync(s => s.Id == id);

        if (society == null) return ApiResponse<Society>.ErrorResponse("Society not found.");
        return ApiResponse<Society>.SuccessResponse(society);
    }

    public async Task<ApiResponse<List<Society>>> GetAllSocietiesAsync()
    {
        var societies = await _context.Societies
            .Include(s => s.LoanTypes)
            .Include(s => s.BankAccounts)
            .Include(s => s.Members) // ✅ Include members
            .ToListAsync();

        return ApiResponse<List<Society>>.SuccessResponse(societies);
    }

    public async Task<ApiResponse<bool>> DeleteSocietyAsync(Guid id)
    {
        var society = await _context.Societies.FindAsync(id);
        if (society == null) return ApiResponse<bool>.ErrorResponse("Society not found.");

        _context.Societies.Remove(society);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, "Society deleted successfully");
    }

    // --- Helpers ---
    private async Task<bool> IsSocietyUniqueAsync(SocietyCreateUpdateDto dto, Guid? id = null)
    {
        return !await _context.Societies.AnyAsync(s =>
            (s.Name == dto.Name ||
             s.Email == dto.Email ||
             s.Phone == dto.Phone ||
             s.Fax == dto.Fax ||
             s.Address == dto.Address ||
             s.Website == dto.Website ||
             s.RegistrationNumber == dto.RegistrationNumber) &&
            (id == null || s.Id != id.Value));
    }

    private bool AreLoanTypesUnique(List<LoanTypeCreateUpdateDto> loanTypes)
    {
        var names = loanTypes.Select(lt => lt.Name.ToLower()).ToList();
        return names.Count == names.Distinct().Count();
    }

    private bool AreBankAccountsUnique(List<BankAccountCreateDto> bankAccounts)
    {
        var keys = bankAccounts.Select(ba => $"{ba.BankName.ToLower()}_{ba.AccountNumber}").ToList();
        return keys.Count == keys.Distinct().Count();
    }

    private void UpdateLoanTypes(Society society, List<LoanTypeCreateUpdateDto> dtos)
    {
        var keepIds = dtos.Where(d => d.LoanTypeId.HasValue).Select(d => d.LoanTypeId!.Value).ToList();
        var toRemove = society.LoanTypes.Where(lt => !keepIds.Contains(lt.LoanTypeId)).ToList();
        _context.LoanTypes.RemoveRange(toRemove);

        foreach (var dto in dtos)
        {
            if (dto.LoanTypeId.HasValue)
            {
                var existing = society.LoanTypes.FirstOrDefault(lt => lt.LoanTypeId == dto.LoanTypeId.Value);
                if (existing != null)
                {
                    existing.Name = dto.Name;
                    existing.InterestPercent = dto.InterestPercent;
                    existing.LimitAmount = dto.LimitAmount;
                    existing.CompulsoryDeposit = dto.CompulsoryDeposit;
                    existing.OptionalDeposit = dto.OptionalDeposit;
                    existing.ShareAmount = dto.ShareAmount;
                    existing.XTimes = dto.XTimes;
                    existing.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    society.LoanTypes.Add(new LoanType
                    {
                        SocietyId = society.Id,
                        Name = dto.Name,
                        InterestPercent = dto.InterestPercent,
                        LimitAmount = dto.LimitAmount,
                        CompulsoryDeposit = dto.CompulsoryDeposit,
                        OptionalDeposit = dto.OptionalDeposit,
                        ShareAmount = dto.ShareAmount,
                        XTimes = dto.XTimes,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
            else
            {
                society.LoanTypes.Add(new LoanType
                {
                    SocietyId = society.Id,
                    Name = dto.Name,
                    InterestPercent = dto.InterestPercent,
                    LimitAmount = dto.LimitAmount,
                    CompulsoryDeposit = dto.CompulsoryDeposit,
                    OptionalDeposit = dto.OptionalDeposit,
                    ShareAmount = dto.ShareAmount,
                    XTimes = dto.XTimes,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
    }

    private void UpdateBankAccounts(Society society, List<BankAccountCreateDto> dtos)
    {
        var keepIds = dtos.Where(d => d.Id.HasValue).Select(d => d.Id!.Value).ToList();
        var toRemove = society.BankAccounts.Where(ba => !keepIds.Contains(ba.Id)).ToList();
        _context.SocietyBankAccounts.RemoveRange(toRemove);

        foreach (var dto in dtos)
        {
            if (dto.Id.HasValue)
            {
                var existing = society.BankAccounts.FirstOrDefault(ba => ba.Id == dto.Id.Value);
                if (existing != null)
                {
                    existing.BankName = dto.BankName;
                    existing.AccountNumber = dto.AccountNumber;
                    existing.IFSC = dto.IFSC;
                    existing.Branch = dto.Branch;
                    existing.Notes = dto.Notes;
                    existing.IsPrimary = dto.IsPrimary;
                    existing.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    society.BankAccounts.Add(new SocietyBankAccount
                    {
                        SocietyId = society.Id,
                        BankName = dto.BankName,
                        AccountNumber = dto.AccountNumber,
                        IFSC = dto.IFSC,
                        Branch = dto.Branch,
                        Notes = dto.Notes,
                        IsPrimary = dto.IsPrimary,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
            else
            {
                society.BankAccounts.Add(new SocietyBankAccount
                {
                    SocietyId = society.Id,
                    BankName = dto.BankName,
                    AccountNumber = dto.AccountNumber,
                    IFSC = dto.IFSC,
                    Branch = dto.Branch,
                    Notes = dto.Notes,
                    IsPrimary = dto.IsPrimary,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
    }

    private void UpdateMembers(Society society, List<MemberCreateUpdateDto>? dtos)
    {
        if (dtos == null) return;

        var toRemove = society.Members
            .Where(m => !dtos.Any(d => d.Email == m.Email)) // remove members not in DTO
            .ToList();
        _context.Members.RemoveRange(toRemove);

        foreach (var dto in dtos)
        {
            var existing = society.Members.FirstOrDefault(m => m.Email == dto.Email);
            if (existing != null)
            {
                existing.Name = dto.Name;
                existing.FHName = dto.FHName;
                existing.Mobile = dto.Mobile;
                existing.Status = dto.Status;
                existing.OfficeAddress = dto.OfficeAddress;
                existing.City = dto.City;
                existing.PhoneOffice = dto.PhoneOffice;
                existing.Branch = dto.Branch;
                existing.PhoneRes = dto.PhoneRes;
                existing.Designation = dto.Designation;
                existing.ResidenceAddress = dto.ResidenceAddress;
                existing.DOB = dto.DOB;
                existing.DOJSociety = dto.DOJSociety;
                existing.DOR = dto.DOR;
                existing.Nominee = dto.Nominee;
                existing.NomineeRelation = dto.NomineeRelation;
                existing.cdAmount = dto.cdAmount;
                existing.Email2 = dto.Email2;
                existing.Mobile2 = dto.Mobile2;
                existing.Pincode = dto.Pincode;
                existing.BankName = dto.BankName;
                existing.AccountNumber = dto.AccountNumber;
                existing.PayableAt = dto.PayableAt;
                existing.Share = dto.Share;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                society.Members.Add(new Member
                {
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
                });
            }
        }
    }
}
