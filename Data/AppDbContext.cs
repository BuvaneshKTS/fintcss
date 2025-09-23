// using Microsoft.EntityFrameworkCore;
// using FintcsApi.Models;

// namespace FintcsApi.Data;

// public class AppDbContext : DbContext
// {
//     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//     {
//     }

//     public DbSet<User> Users { get; set; }
//     public DbSet<Society> Societies { get; set; }
//     public DbSet<LoanType> LoanTypes { get; set; }
//     public DbSet<SocietyBankAccount> SocietyBankAccounts { get; set; }

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         base.OnModelCreating(modelBuilder);

//         // Configure User entity
//         modelBuilder.Entity<User>(entity =>
//         {
//             entity.HasIndex(e => e.Username).IsUnique();
//             entity.HasIndex(e => e.Email).IsUnique();
//         });

//         // Configure Society entity
//         modelBuilder.Entity<Society>(entity =>
//         {
//             entity.Property(e => e.ChequeBounceCharge)
//                 .HasPrecision(18, 2);
//         });

//         // Configure LoanType entity
//         modelBuilder.Entity<LoanType>(entity =>
//         {
//             entity.Property(e => e.InterestPercent)
//                 .HasPrecision(5, 2);
//             entity.Property(e => e.LimitAmount)
//                 .HasPrecision(18, 2);
//             entity.Property(e => e.CompulsoryDeposit)
//                 .HasPrecision(18, 2);
//             entity.Property(e => e.OptionalDeposit)
//                 .HasPrecision(18, 2);
//             entity.Property(e => e.ShareAmount)
//                 .HasPrecision(18, 2);

//             entity.HasOne(e => e.Society)
//                 .WithMany(s => s.LoanTypes)
//                 .HasForeignKey(e => e.SocietyId)
//                 .OnDelete(DeleteBehavior.Cascade);
//         });

//         // Configure SocietyBankAccount entity
//         modelBuilder.Entity<SocietyBankAccount>(entity =>
//         {
//             entity.HasOne(e => e.Society)
//                 .WithMany(s => s.BankAccounts)
//                 .HasForeignKey(e => e.SocietyId)
//                 .OnDelete(DeleteBehavior.Cascade);
//         });
//     }
// }


using FintcsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FintcsApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }   // <-- re-add this
        public DbSet<Society> Societies { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<SocietyBankAccount> SocietyBankAccounts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User uniqueness
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Society uniqueness constraints
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.Email).IsUnique();
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.Phone).IsUnique();
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.Fax).IsUnique();
            // modelBuilder.Entity<Society>()
            //     .HasIndex(s => s.Address).IsUnique();
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.Website).IsUnique();
            modelBuilder.Entity<Society>()
                .HasIndex(s => s.RegistrationNumber).IsUnique();

            // LoanType uniqueness per society
            modelBuilder.Entity<LoanType>()
                .HasIndex(lt => new { lt.SocietyId, lt.Name }).IsUnique();

            // BankAccount uniqueness per society
            modelBuilder.Entity<SocietyBankAccount>()
                .HasIndex(ba => new { ba.SocietyId, ba.BankName, ba.AccountNumber }).IsUnique();
        }
    }
}
