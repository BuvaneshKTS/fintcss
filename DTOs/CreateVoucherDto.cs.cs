using System;
using System.Collections.Generic;

namespace FintcsApi.DTOs
{
    public class CreateVoucherDto
    {
        public string VoucherType { get; set; } = null!;
        public Guid? MemberId { get; set; }
        public Guid? LoanId { get; set; }
        public string? Narration { get; set; }

        public List<VoucherEntryDto> Entries { get; set; } = new();
    }

    public class VoucherEntryDto
    {
        public Guid LedgerAccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string? Description { get; set; }
    }
}
