using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.Transactions.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class TransferMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Transactions");

            migrationBuilder.CreateTable(
                name: "Transfers",
                schema: "Transactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    WalletId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CounterPartyId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Direction = table.Column<int>(type: "int", nullable: true),
                    BankTransferType = table.Column<int>(type: "int", nullable: true),
                    ReciverId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_CounterPartyId",
                schema: "Transactions",
                table: "Transfers",
                column: "CounterPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_Number",
                schema: "Transactions",
                table: "Transfers",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ReciverId",
                schema: "Transactions",
                table: "Transfers",
                column: "ReciverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers",
                schema: "Transactions");
        }
    }
}
