using System;
using System.ComponentModel.DataAnnotations;

namespace FintcsApi.Models
{
    public class Loan
    {
        [Key]
        public Guid LoanId { get; set; }

        [Required]
        public Guid SocietyId { get; set; }
        public Society? Society { get; set; }

        [Required]
        public Guid MemberId { get; set; }
        public Member? Member { get; set; }

        [Required]
        public Guid LoanTypeId { get; set; }
        public LoanType? LoanType { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        public decimal LoanAmount { get; set; }
        public decimal PreviousLoan { get; set; }   // reference if needed
        public int Installments { get; set; }
        public string? Purpose { get; set; }
        public string? AuthorizedBy { get; set; }
        public string PaymentMode { get; set; } = string.Empty; // Cash, Cheque, Transfer
        public string? Bank { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string Status { get; set; } = "Pending";

        // Derived/extra amounts
        public decimal NetLoan { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal NewLoanShare { get; set; }
        public decimal PayAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
