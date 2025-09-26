namespace FintcsApi.DTOs;

public class SocietyDto : SocietyCreateUpdateDto
{
    public Guid Id { get; set; }

    // Only include related entities when needed
    public List<BankAccountDto>? BankAccounts { get; set; }
    public List<LoanTypeDto>? LoanTypes { get; set; }
}
