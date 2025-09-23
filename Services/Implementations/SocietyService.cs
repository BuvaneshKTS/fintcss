// // File: FintcsApi/Services/Implementations/SocietyService.cs 
// using Microsoft.EntityFrameworkCore;
// using FintcsApi.Data;
// using FintcsApi.DTOs;
// using FintcsApi.Models;
// using FintcsApi.Services.Interfaces;

// namespace FintcsApi.Services.Implementations;

// public class SocietyService : ISocietyService
// {
//     private readonly AppDbContext _context;

//     public SocietyService(AppDbContext context)
//     {
//         _context = context;
//     }

//     public async Task<ApiResponse<Society>> CreateSocietyAsync(SocietyCreateUpdateDto societyDto)
//     {
//         try
//         {
//             var society = new Society
//             {
//                 Name = societyDto.Name,
//                 Address = societyDto.Address,
//                 City = societyDto.City,
//                 Phone = societyDto.Phone,
//                 Fax = societyDto.Fax,
//                 Email = societyDto.Email,
//                 Website = societyDto.Website,
//                 RegistrationNumber = societyDto.RegistrationNumber,
//                 ChequeBounceCharge = societyDto.ChequeBounceCharge,
//                 CreatedAt = DateTime.UtcNow
//             };

//             // Add loan types
//             foreach (var loanTypeDto in societyDto.LoanTypes)
//             {
//                 var loanType = new LoanType
//                 {
//                     SocietyId = society.Id,
//                     Name = loanTypeDto.Name,
//                     InterestPercent = loanTypeDto.InterestPercent,
//                     LimitAmount = loanTypeDto.LimitAmount,
//                     CompulsoryDeposit = loanTypeDto.CompulsoryDeposit,
//                     OptionalDeposit = loanTypeDto.OptionalDeposit,
//                     ShareAmount = loanTypeDto.ShareAmount,
//                     XTimes = loanTypeDto.XTimes,
//                     CreatedAt = DateTime.UtcNow
//                 };
//                 society.LoanTypes.Add(loanType);
//             }

//             // Add bank accounts
//             foreach (var bankAccountDto in societyDto.BankAccounts)
//             {
//                 var bankAccount = new SocietyBankAccount
//                 {
//                     SocietyId = society.Id,
//                     BankName = bankAccountDto.BankName,
//                     AccountNumber = bankAccountDto.AccountNumber,
//                     IFSC = bankAccountDto.IFSC,
//                     Branch = bankAccountDto.Branch,
//                     Notes = bankAccountDto.Notes,
//                     IsPrimary = bankAccountDto.IsPrimary,
//                     CreatedAt = DateTime.UtcNow
//                 };
//                 society.BankAccounts.Add(bankAccount);
//             }

//             _context.Societies.Add(society);
//             await _context.SaveChangesAsync();

//             // Load the society with all relationships
//             var createdSociety = await _context.Societies
//                 .Include(s => s.LoanTypes)
//                 .Include(s => s.BankAccounts)
//                 .FirstAsync(s => s.Id == society.Id);

//             return ApiResponse<Society>.SuccessResponse(createdSociety, "Society created successfully");
//         }
//         catch (Exception ex)
//         {
//             return ApiResponse<Society>.ErrorResponse($"Error creating society: {ex.Message}");
//         }
//     }

//     public async Task<ApiResponse<Society>> GetSocietyByIdAsync(Guid id)
//     {
//         try
//         {
//             var society = await _context.Societies
//                 .Include(s => s.LoanTypes)
//                 .Include(s => s.BankAccounts)
//                 .FirstOrDefaultAsync(s => s.Id == id);

//             if (society == null)
//             {
//                 return ApiResponse<Society>.ErrorResponse("Society not found");
//             }

//             return ApiResponse<Society>.SuccessResponse(society);
//         }
//         catch (Exception ex)
//         {
//             return ApiResponse<Society>.ErrorResponse($"Error retrieving society: {ex.Message}");
//         }
//     }

//     public async Task<ApiResponse<List<Society>>> GetAllSocietiesAsync()
//     {
//         try
//         {
//             var societies = await _context.Societies
//                 .Include(s => s.LoanTypes)
//                 .Include(s => s.BankAccounts)
//                 .ToListAsync();

//             return ApiResponse<List<Society>>.SuccessResponse(societies);
//         }
//         catch (Exception ex)
//         {
//             return ApiResponse<List<Society>>.ErrorResponse($"Error retrieving societies: {ex.Message}");
//         }
//     }

//     public async Task<ApiResponse<Society>> UpdateSocietyAsync(Guid id, SocietyCreateUpdateDto societyDto)
//     {
//         try
//         {
//             var society = await _context.Societies
//                 .Include(s => s.LoanTypes)
//                 .Include(s => s.BankAccounts)
//                 .FirstOrDefaultAsync(s => s.Id == id);

//             if (society == null)
//             {
//                 return ApiResponse<Society>.ErrorResponse("Society not found");
//             }

//             // Update society properties
//             society.Name = societyDto.Name;
//             society.Address = societyDto.Address;
//             society.City = societyDto.City;
//             society.Phone = societyDto.Phone;
//             society.Fax = societyDto.Fax;
//             society.Email = societyDto.Email;
//             society.Website = societyDto.Website;
//             society.RegistrationNumber = societyDto.RegistrationNumber;
//             society.ChequeBounceCharge = societyDto.ChequeBounceCharge;
//             society.UpdatedAt = DateTime.UtcNow;

//             // Update loan types
//             await UpdateLoanTypesAsync(society, societyDto.LoanTypes);

//             // Update bank accounts
//             await UpdateBankAccountsAsync(society, societyDto.BankAccounts);

//             await _context.SaveChangesAsync();

//             // Reload the society with updated relationships
//             var updatedSociety = await _context.Societies
//                 .Include(s => s.LoanTypes)
//                 .Include(s => s.BankAccounts)
//                 .FirstAsync(s => s.Id == society.Id);

//             return ApiResponse<Society>.SuccessResponse(updatedSociety, "Society updated successfully");
//         }
//         catch (Exception ex)
//         {
//             return ApiResponse<Society>.ErrorResponse($"Error updating society: {ex.Message}");
//         }
//     }

//     public async Task<ApiResponse<bool>> DeleteSocietyAsync(Guid id)
//     {
//         try
//         {
//             var society = await _context.Societies.FindAsync(id);
//             if (society == null)
//             {
//                 return ApiResponse<bool>.ErrorResponse("Society not found");
//             }

//             _context.Societies.Remove(society);
//             await _context.SaveChangesAsync();

//             return ApiResponse<bool>.SuccessResponse(true, "Society deleted successfully");
//         }
//         catch (Exception ex)
//         {
//             return ApiResponse<bool>.ErrorResponse($"Error deleting society: {ex.Message}");
//         }
//     }

//     private async Task UpdateLoanTypesAsync(Society society, List<LoanTypeCreateUpdateDto> loanTypeDtos)
//     {
//         // Remove loan types that are not in the update list
//         var loanTypeIdsToKeep = loanTypeDtos.Where(lt => lt.LoanTypeId.HasValue).Select(lt => lt.LoanTypeId!.Value).ToList();
//         var loanTypesToRemove = society.LoanTypes.Where(lt => !loanTypeIdsToKeep.Contains(lt.LoanTypeId)).ToList();
        
//         foreach (var loanType in loanTypesToRemove)
//         {
//             _context.LoanTypes.Remove(loanType);
//         }

//         // Update existing and add new loan types
//         foreach (var loanTypeDto in loanTypeDtos)
//         {
//             if (loanTypeDto.LoanTypeId.HasValue)
//             {
//                 // Update existing loan type
//                 var existingLoanType = society.LoanTypes.FirstOrDefault(lt => lt.LoanTypeId == loanTypeDto.LoanTypeId.Value);
//                 if (existingLoanType != null)
//                 {
//                     existingLoanType.Name = loanTypeDto.Name;
//                     existingLoanType.InterestPercent = loanTypeDto.InterestPercent;
//                     existingLoanType.LimitAmount = loanTypeDto.LimitAmount;
//                     existingLoanType.CompulsoryDeposit = loanTypeDto.CompulsoryDeposit;
//                     existingLoanType.OptionalDeposit = loanTypeDto.OptionalDeposit;
//                     existingLoanType.ShareAmount = loanTypeDto.ShareAmount;
//                     existingLoanType.XTimes = loanTypeDto.XTimes;
//                     existingLoanType.UpdatedAt = DateTime.UtcNow;
//                 }
//             }
//             else
//             {
//                 // Add new loan type
//                 var newLoanType = new LoanType
//                 {
//                     SocietyId = society.Id,
//                     Name = loanTypeDto.Name,
//                     InterestPercent = loanTypeDto.InterestPercent,
//                     LimitAmount = loanTypeDto.LimitAmount,
//                     CompulsoryDeposit = loanTypeDto.CompulsoryDeposit,
//                     OptionalDeposit = loanTypeDto.OptionalDeposit,
//                     ShareAmount = loanTypeDto.ShareAmount,
//                     XTimes = loanTypeDto.XTimes,
//                     CreatedAt = DateTime.UtcNow
//                 };
//                 society.LoanTypes.Add(newLoanType);
//             }
//         }
//     }

//     private async Task UpdateBankAccountsAsync(Society society, List<BankAccountCreateDto> bankAccountDtos)
//     {
//         // Remove bank accounts that are not in the update list
//         var bankAccountIdsToKeep = bankAccountDtos.Where(ba => ba.Id.HasValue).Select(ba => ba.Id!.Value).ToList();
//         var bankAccountsToRemove = society.BankAccounts.Where(ba => !bankAccountIdsToKeep.Contains(ba.Id)).ToList();
        
//         foreach (var bankAccount in bankAccountsToRemove)
//         {
//             _context.SocietyBankAccounts.Remove(bankAccount);
//         }

//         // Update existing and add new bank accounts
//         foreach (var bankAccountDto in bankAccountDtos)
//         {
//             if (bankAccountDto.Id.HasValue)
//             {
//                 // Update existing bank account
//                 var existingBankAccount = society.BankAccounts.FirstOrDefault(ba => ba.Id == bankAccountDto.Id.Value);
//                 if (existingBankAccount != null)
//                 {
//                     existingBankAccount.BankName = bankAccountDto.BankName;
//                     existingBankAccount.AccountNumber = bankAccountDto.AccountNumber;
//                     existingBankAccount.IFSC = bankAccountDto.IFSC;
//                     existingBankAccount.Branch = bankAccountDto.Branch;
//                     existingBankAccount.Notes = bankAccountDto.Notes;
//                     existingBankAccount.IsPrimary = bankAccountDto.IsPrimary;
//                     existingBankAccount.UpdatedAt = DateTime.UtcNow;
//                 }
//             }
//             else
//             {
//                 // Add new bank account
//                 var newBankAccount = new SocietyBankAccount
//                 {
//                     SocietyId = society.Id,
//                     BankName = bankAccountDto.BankName,
//                     AccountNumber = bankAccountDto.AccountNumber,
//                     IFSC = bankAccountDto.IFSC,
//                     Branch = bankAccountDto.Branch,
//                     Notes = bankAccountDto.Notes,
//                     IsPrimary = bankAccountDto.IsPrimary,
//                     CreatedAt = DateTime.UtcNow
//                 };
//                 society.BankAccounts.Add(newBankAccount);
//             }
//         }
//     }
// }


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

            _context.Societies.Add(society);
            await _context.SaveChangesAsync();

            var created = await _context.Societies
                .Include(s => s.LoanTypes)
                .Include(s => s.BankAccounts)
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

            await _context.SaveChangesAsync();

            var updated = await _context.Societies
                .Include(s => s.LoanTypes)
                .Include(s => s.BankAccounts)
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
            .FirstOrDefaultAsync(s => s.Id == id);

        if (society == null) return ApiResponse<Society>.ErrorResponse("Society not found.");
        return ApiResponse<Society>.SuccessResponse(society);
    }

    public async Task<ApiResponse<List<Society>>> GetAllSocietiesAsync()
    {
        var societies = await _context.Societies
            .Include(s => s.LoanTypes)
            .Include(s => s.BankAccounts)
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
            }
            else
            {
                society.LoanTypes.Add(new LoanType
                {
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
            }
            else
            {
                society.BankAccounts.Add(new SocietyBankAccount
                {
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
}
