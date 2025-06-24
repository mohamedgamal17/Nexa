using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.Accounting.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ResservedBalanceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ReservedBalance",
                schema: "Accounting",
                table: "Wallet",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedBalance",
                schema: "Accounting",
                table: "Wallet");
        }
    }
}
