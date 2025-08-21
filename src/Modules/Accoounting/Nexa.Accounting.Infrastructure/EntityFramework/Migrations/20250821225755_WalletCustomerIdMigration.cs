using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.Accounting.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class WalletCustomerIdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                schema: "Accounting",
                table: "Wallet",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "State",
                schema: "Accounting",
                table: "Wallet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_CustomerId",
                schema: "Accounting",
                table: "Wallet",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallet_CustomerId",
                schema: "Accounting",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "Accounting",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "Accounting",
                table: "Wallet");
        }
    }
}
