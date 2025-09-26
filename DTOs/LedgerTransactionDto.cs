using System;

namespace FintcsApi.DTOs
{
    public class LedgerTransactionDto
    {
        public Guid LedgerAccountId { get; set; }
        public Guid? MemberId { get; set; }
        public Guid? LoanId { get; set; }

        public decimal Debit { get; set; } = 0;
        public decimal Credit { get; set; } = 0;

        public Guid SocietyId { get; set; }
        public Guid? BankId { get; set; }

        public string Description { get; set; } = "";
    }
}
