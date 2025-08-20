using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RenameCustomerFintechCustomerIdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FintechCustomerid",
                schema: "CustomerManagement",
                table: "Customers",
                newName: "FintechCustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FintechCustomerId",
                schema: "CustomerManagement",
                table: "Customers",
                newName: "FintechCustomerid");
        }
    }
}
