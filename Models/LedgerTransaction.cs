// Models/LedgerTransaction.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FintcsApi.Models
{
    public class LedgerTransaction
    {
        [Key]
        public Guid LedgerTransactionId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid LedgerAccountId { get; set; }

        [ForeignKey("LedgerAccountId")]
        public LedgerAccount LedgerAccount { get; set; } = null!;

        public Guid? MemberId { get; set; }
        public Guid? LoanId { get; set; }

        public decimal Debit { get; set; } = 0;
        public decimal Credit { get; set; } = 0;
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 👇 Link back to voucher
        public Guid? VoucherId { get; set; }
        
        [ForeignKey("VoucherId")]
        public Voucher? Voucher { get; set; }
    }
}
