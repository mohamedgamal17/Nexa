using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class FixCustomerApplicationPhoneNumberLengthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "CustomerManagement",
                table: "CustomersApplications",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "CustomerManagement",
                table: "CustomersApplications",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(35)",
                oldMaxLength: 35);
        }
    }
}
