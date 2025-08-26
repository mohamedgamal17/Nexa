using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.Accounting.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class BankAccountMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BanKAccounts",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProviderBankAccountId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    HolderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    AccountNumberLast4 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    RoutingNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanKAccounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BanKAccounts_CustomerId",
                schema: "Accounting",
                table: "BanKAccounts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BanKAccounts_ProviderBankAccountId",
                schema: "Accounting",
                table: "BanKAccounts",
                column: "ProviderBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BanKAccounts_UserId",
                schema: "Accounting",
                table: "BanKAccounts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BanKAccounts",
                schema: "Accounting");
        }
    }
}
