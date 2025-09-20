using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.Transactions.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ExternalTransferMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalTransferId",
                schema: "Transactions",
                table: "Transfers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ExternalTransferId",
                schema: "Transactions",
                table: "Transfers",
                column: "ExternalTransferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transfers_ExternalTransferId",
                schema: "Transactions",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "ExternalTransferId",
                schema: "Transactions",
                table: "Transfers");
        }
    }
}
