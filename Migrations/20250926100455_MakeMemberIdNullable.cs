using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FintcsApi.Migrations
{
    /// <inheritdoc />
    public partial class MakeMemberIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerAccounts_Members_MemberId",
                table: "LedgerAccounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "MemberId",
                table: "LedgerAccounts",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerAccounts_Members_MemberId",
                table: "LedgerAccounts",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LedgerAccounts_Members_MemberId",
                table: "LedgerAccounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "MemberId",
                table: "LedgerAccounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LedgerAccounts_Members_MemberId",
                table: "LedgerAccounts",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
