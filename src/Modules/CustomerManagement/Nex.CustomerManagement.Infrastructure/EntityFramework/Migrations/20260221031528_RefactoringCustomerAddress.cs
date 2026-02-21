using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringCustomerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_City",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_State",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_StreetLine",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_City",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_State",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_StreetLine",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                schema: "CustomerManagement",
                table: "OnboardCustomers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                schema: "CustomerManagement",
                table: "OnboardCustomers",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                schema: "CustomerManagement",
                table: "OnboardCustomers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                schema: "CustomerManagement",
                table: "OnboardCustomers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_StreetLine",
                schema: "CustomerManagement",
                table: "OnboardCustomers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                schema: "CustomerManagement",
                table: "OnboardCustomers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "CustomerManagement",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StreetLine = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Addresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "CustomerManagement",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "CustomerManagement");

            migrationBuilder.DropColumn(
                name: "Address_City",
                schema: "CustomerManagement",
                table: "OnboardCustomers");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                schema: "CustomerManagement",
                table: "OnboardCustomers");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                schema: "CustomerManagement",
                table: "OnboardCustomers");

            migrationBuilder.DropColumn(
                name: "Address_State",
                schema: "CustomerManagement",
                table: "OnboardCustomers");

            migrationBuilder.DropColumn(
                name: "Address_StreetLine",
                schema: "CustomerManagement",
                table: "OnboardCustomers");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                schema: "CustomerManagement",
                table: "OnboardCustomers");

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_StreetLine",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                schema: "CustomerManagement",
                table: "OnboardCustomersInfos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_StreetLine",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
