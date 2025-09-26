    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using FintcsApi.Data;
    using FintcsApi.DTOs;
    using FintcsApi.Models;
    using FintcsApi.Services.Interfaces;

    namespace FintcsApi.Services.Implementations
    {
        public class VoucherService : IVoucherService
        {
            private readonly AppDbContext _context;

            public VoucherService(AppDbContext context)
            {
                _context = context;
            }

            public async Task<Voucher> CreateVoucherAsync(CreateVoucherDto dto)
            {
                using var dbTransaction = await _context.Database.BeginTransactionAsync();

                var voucher = new Voucher
                {
                    VoucherId = Guid.NewGuid(),
                    VoucherNumber = $"VCH-{DateTime.UtcNow:yyyyMMddHHmmss}",
                    VoucherType = dto.VoucherType,
                    MemberId = dto.MemberId,
                    LoanId = dto.LoanId,
                    Narration = dto.Narration
                };

                _context.Vouchers.Add(voucher);
                await _context.SaveChangesAsync();

                foreach (var entry in dto.Entries)
                {
                    var ledger = await _context.LedgerAccounts.FindAsync(entry.LedgerAccountId);
                    if (ledger == null)
                        throw new Exception("Ledger account not found");

                    ledger.Balance += entry.Credit - entry.Debit;
                    _context.LedgerAccounts.Update(ledger);

                    _context.LedgerTransactions.Add(new LedgerTransaction
                    {
                        LedgerTransactionId = Guid.NewGuid(),
                        VoucherId = voucher.VoucherId,
                        LedgerAccountId = entry.LedgerAccountId,
                        MemberId = dto.MemberId,
                        LoanId = dto.LoanId,
                        Debit = entry.Debit,
                        Credit = entry.Credit,
                        Description = entry.Description
                    });
                }

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return voucher;
            }
        }
    }
