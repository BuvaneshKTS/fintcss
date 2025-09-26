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

    // Create society
    public async Task<ApiResponse<SocietyDto>> CreateSocietyAsync(SocietyCreateUpdateDto dto)
    {
        try
        {
            if (!await IsSocietyUniqueAsync(dto))
                return ApiResponse<SocietyDto>.ErrorResponse(
                    "Society with same Name, Email, Phone, Fax, Address, Website or RegistrationNumber already exists.");

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

            _context.Societies.Add(society);
            await _context.SaveChangesAsync();

            return await GetSocietyByIdAsync(society.Id);
        }
        catch (Exception ex)
        {
            return ApiResponse<SocietyDto>.ErrorResponse($"Error creating society: {ex.Message}");
        }
    }

    // Get society by Id (includes LoanTypes + BankAccounts only)
    public async Task<ApiResponse<SocietyDto>> GetSocietyByIdAsync(Guid id)
    {
        var society = await _context.Societies
            .Include(s => s.LoanTypes)
            .Include(s => s.BankAccounts)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (society == null)
            return ApiResponse<SocietyDto>.ErrorResponse("Society not found.");

        var dto = new SocietyDto
        {
            Id = society.Id,
            Name = society.Name,
            Address = society.Address,
            City = society.City,
            Phone = society.Phone,
            Fax = society.Fax,
            Email = society.Email,
            Website = society.Website,
            RegistrationNumber = society.RegistrationNumber,
            ChequeBounceCharge = society.ChequeBounceCharge,
            LoanTypes = society.LoanTypes.Select(lt => new LoanTypeDto
            {
                LoanTypeId = lt.LoanTypeId,
                SocietyId = lt.SocietyId,
                Name = lt.Name,
                InterestPercent = lt.InterestPercent,
                LimitAmount = lt.LimitAmount,
                CompulsoryDeposit = lt.CompulsoryDeposit,
                OptionalDeposit = lt.OptionalDeposit,
                ShareAmount = lt.ShareAmount,
                XTimes = lt.XTimes
            }).ToList(),
            BankAccounts = society.BankAccounts.Select(ba => new BankAccountDto
            {
                Id = ba.Id,
                BankName = ba.BankName,
                AccountNumber = ba.AccountNumber,
                IFSC = ba.IFSC,
                Branch = ba.Branch,
                Notes = ba.Notes,
                IsPrimary = ba.IsPrimary
            }).ToList()
        };

        return ApiResponse<SocietyDto>.SuccessResponse(dto);
    }

    // Get all societies (without members)
    public async Task<ApiResponse<List<SocietyDto>>> GetAllSocietiesAsync()
    {
        var societies = await _context.Societies
            .Include(s => s.LoanTypes)
            .Include(s => s.BankAccounts)
            .ToListAsync();

        var dtos = societies.Select(s => new SocietyDto
        {
            Id = s.Id,
            Name = s.Name,
            Address = s.Address,
            City = s.City,
            Phone = s.Phone,
            Fax = s.Fax,
            Email = s.Email,
            Website = s.Website,
            RegistrationNumber = s.RegistrationNumber,
            ChequeBounceCharge = s.ChequeBounceCharge,
            LoanTypes = s.LoanTypes.Select(lt => new LoanTypeDto
            {
                LoanTypeId = lt.LoanTypeId,
                SocietyId = lt.SocietyId,
                Name = lt.Name,
                InterestPercent = lt.InterestPercent,
                LimitAmount = lt.LimitAmount,
                CompulsoryDeposit = lt.CompulsoryDeposit,
                OptionalDeposit = lt.OptionalDeposit,
                ShareAmount = lt.ShareAmount,
                XTimes = lt.XTimes
            }).ToList(),
            BankAccounts = s.BankAccounts.Select(ba => new BankAccountDto
            {
                Id = ba.Id,
                BankName = ba.BankName,
                AccountNumber = ba.AccountNumber,
                IFSC = ba.IFSC,
                Branch = ba.Branch,
                Notes = ba.Notes,
                IsPrimary = ba.IsPrimary
            }).ToList()
        }).ToList();

        return ApiResponse<List<SocietyDto>>.SuccessResponse(dtos);
    }

    // Update society
    public async Task<ApiResponse<SocietyDto>> UpdateSocietyAsync(Guid id, SocietyCreateUpdateDto dto)
    {
        var society = await _context.Societies.FindAsync(id);
        if (society == null) return ApiResponse<SocietyDto>.ErrorResponse("Society not found.");

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

        await _context.SaveChangesAsync();

        return await GetSocietyByIdAsync(society.Id);
    }

    // Delete society
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
            (s.Name == dto.Name || s.Email == dto.Email || s.Phone == dto.Phone ||
             s.Fax == dto.Fax || s.Address == dto.Address || s.Website == dto.Website ||
             s.RegistrationNumber == dto.RegistrationNumber) &&
            (id == null || s.Id != id.Value));
    }
}
