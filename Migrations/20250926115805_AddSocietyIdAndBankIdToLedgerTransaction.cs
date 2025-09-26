using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FintcsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSocietyIdAndBankIdToLedgerTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "LedgerTransactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SocietyId",
                table: "LedgerTransactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankId",
                table: "LedgerTransactions");

            migrationBuilder.DropColumn(
                name: "SocietyId",
                table: "LedgerTransactions");
        }
    }
}
