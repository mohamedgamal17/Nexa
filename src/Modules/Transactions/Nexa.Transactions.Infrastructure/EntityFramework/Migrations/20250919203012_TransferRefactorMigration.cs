using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.Transactions.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class TransferRefactorMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CounterPartyId",
                schema: "Transactions",
                table: "Transfers",
                newName: "FundingResourceId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_CounterPartyId",
                schema: "Transactions",
                table: "Transfers",
                newName: "IX_Transfers_FundingResourceId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "Transactions",
                table: "Transfers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_UserId",
                schema: "Transactions",
                table: "Transfers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transfers_UserId",
                schema: "Transactions",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Transactions",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "FundingResourceId",
                schema: "Transactions",
                table: "Transfers",
                newName: "CounterPartyId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_FundingResourceId",
                schema: "Transactions",
                table: "Transfers",
                newName: "IX_Transfers_CounterPartyId");
        }
    }
}
