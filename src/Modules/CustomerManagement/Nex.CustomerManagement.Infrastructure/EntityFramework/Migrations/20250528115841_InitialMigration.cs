using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CustomerManagement");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "CustomerManagement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    KYCExternalId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Address_Country = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address_StreetLine1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Address_StreetLine2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address_PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address_ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                schema: "CustomerManagement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    KYCExternalId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IssuingCountry = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Document_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "CustomerManagement",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentAttachment",
                schema: "CustomerManagement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    KYCExternalId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentAttachment_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "CustomerManagement",
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_KYCExternalId",
                schema: "CustomerManagement",
                table: "Customers",
                column: "KYCExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                schema: "CustomerManagement",
                table: "Customers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_CustomerId",
                schema: "CustomerManagement",
                table: "Document",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_KYCExternalId",
                schema: "CustomerManagement",
                table: "Document",
                column: "KYCExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_UserId",
                schema: "CustomerManagement",
                table: "Document",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAttachment_DocumentId",
                schema: "CustomerManagement",
                table: "DocumentAttachment",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAttachment_KYCExternalId",
                schema: "CustomerManagement",
                table: "DocumentAttachment",
                column: "KYCExternalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentAttachment",
                schema: "CustomerManagement");

            migrationBuilder.DropTable(
                name: "Document",
                schema: "CustomerManagement");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "CustomerManagement");
        }
    }
}
