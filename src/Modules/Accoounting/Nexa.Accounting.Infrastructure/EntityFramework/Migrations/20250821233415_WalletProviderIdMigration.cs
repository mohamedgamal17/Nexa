using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.Accounting.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class WalletProviderIdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProviderWalletId",
                schema: "Accounting",
                table: "Wallet",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_ProviderWalletId",
                schema: "Accounting",
                table: "Wallet",
                column: "ProviderWalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallet_ProviderWalletId",
                schema: "Accounting",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "ProviderWalletId",
                schema: "Accounting",
                table: "Wallet");
        }
    }
}
