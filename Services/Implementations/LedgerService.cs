// Services/Implementations/LedgerService.cs
using FintcsApi.Data;
using FintcsApi.DTOs;
using FintcsApi.Models;
using FintcsApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FintcsApi.Services.Implementations
{
    public class LedgerService : ILedgerService
    {
        private readonly AppDbContext _context;
        

        public LedgerService(AppDbContext context)
        {
            _context = context;
        }

        // Create all default ledgers for a new member
        public async Task CreateDefaultLedgersForMemberAsync(Guid memberId)
        {
            string[] defaultLedgers = new string[]
            {
                "Admission Fee Ledger",
                "Building Fund Ledger",
                "CD Ledger",
                "OD Ledger",
                "Share Ledger"
            };

            foreach (var ledgerName in defaultLedgers)
            {
                if (!await _context.LedgerAccounts.AnyAsync(l => l.MemberId == memberId && l.AccountName == ledgerName))
                {
                    _context.LedgerAccounts.Add(new LedgerAccount
                    {
                        MemberId = memberId,
                        AccountName = ledgerName,
                        Balance = 0
                    });
                }
            }

            // Add loan type ledgers dynamically
            var loanTypes = await _context.LoanTypes.ToListAsync();
            foreach (var loanType in loanTypes)
            {
                string loanLedgerName = $"{loanType.Name} Loan Ledger";
                if (!await _context.LedgerAccounts.AnyAsync(l => l.MemberId == memberId && l.AccountName == loanLedgerName))
                {
                    _context.LedgerAccounts.Add(new LedgerAccount
                    {
                        MemberId = memberId,
                        AccountName = loanLedgerName,
                        Balance = 0
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        // Record a transaction (double entry)
        public async Task RecordTransactionAsync(LedgerTransactionDto dto)
        {
            var ledger = await _context.LedgerAccounts.FindAsync(dto.LedgerAccountId);
            if (ledger == null) throw new Exception("Ledger account not found");

            // Update balance
            ledger.Balance = ledger.Balance + dto.Credit - dto.Debit;
            _context.LedgerAccounts.Update(ledger);

            // Add transaction and get the saved entity
            var transaction = new LedgerTransaction
            {
                LedgerAccountId = dto.LedgerAccountId,
                MemberId = dto.MemberId,
                LoanId = dto.LoanId,
                Debit = dto.Debit,
                Credit = dto.Credit,
                Description = dto.Description,
                TransactionDate = DateTime.UtcNow
            };

            _context.LedgerTransactions.Add(transaction);
            await _context.SaveChangesAsync(); // Save to get LedgerTransactionId

            // Create voucher for this transaction
            var voucher = new Voucher
            {
                LedgerTransactionId = transaction.LedgerTransactionId,
                VoucherNumber = $"VCH-{DateTime.UtcNow:yyyyMMddHHmmssfff}", // unique voucher number
                VoucherType = dto.Debit > 0 ? "Debit" : "Credit",
                VoucherDate = transaction.TransactionDate,
                Narration = dto.Description,
                MemberId = dto.MemberId,
                LoanId = dto.LoanId
            };

            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
        }

        // Create other ledger (EB, Telephone, Misc.)
        public async Task CreateOtherLedgerAsync(Guid? memberId, string accountName, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentException("Account name is required");

            bool exists = await _context.LedgerAccounts
                .AnyAsync(l => l.MemberId == memberId && l.AccountName == accountName);

            if (exists)
                throw new Exception("Ledger account already exists");

            var ledger = new LedgerAccount
            {
                MemberId = memberId, // null is fine for society-level ledgers
                AccountName = accountName,
                Balance = initialBalance
            };


            _context.LedgerAccounts.Add(ledger);
            await _context.SaveChangesAsync();
        }

// Record transaction for other ledger
        public async Task RecordOtherLedgerTransactionAsync(LedgerTransactionDto dto)
        {
            var ledger = await _context.LedgerAccounts.FindAsync(dto.LedgerAccountId);
            if (ledger == null) throw new Exception("Ledger account not found");

            // Update balance
            ledger.Balance = ledger.Balance + dto.Credit - dto.Debit;
            _context.LedgerAccounts.Update(ledger);

            // Create transaction
            var transaction = new LedgerTransaction
            {
                LedgerAccountId = dto.LedgerAccountId,
                MemberId = dto.MemberId,
                LoanId = dto.LoanId,
                Debit = dto.Debit,
                Credit = dto.Credit,
                SocietyId = dto.SocietyId,
                BankId = dto.BankId,
                Description = dto.Description,
                TransactionDate = DateTime.UtcNow
            };

            _context.LedgerTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Optionally, create a voucher
            var voucher = new Voucher
            {
                LedgerTransactionId = transaction.LedgerTransactionId,
                VoucherNumber = $"VCH-{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                VoucherType = dto.Debit > 0 ? "Debit" : "Credit",
                VoucherDate = transaction.TransactionDate,
                Narration = dto.Description,
                MemberId = dto.MemberId,
                LoanId = dto.LoanId
            };

            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
        }

    }
}
