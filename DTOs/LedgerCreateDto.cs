using System;

namespace FintcsApi.DTOs
{
    public class LedgerCreateDto
    {
        public Guid? MemberId { get; set; }
        public Guid? SocietyId { get; set; }   // ✅ Link to Society
        public string AccountName { get; set; } = null!;
        public string AccountType { get; set; } = "General"; // e.g. Share, Loan, Cash, Bank
        public decimal InitialBalance { get; set; } = 0;
    }
}
