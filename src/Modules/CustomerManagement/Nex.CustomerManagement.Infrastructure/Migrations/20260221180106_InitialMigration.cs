using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.CustomerManagement.Infrastructure.Migrations
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
                    KycCustomerId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FintechCustomerId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnboardCustomers",
                schema: "CustomerManagement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardCustomers", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "CustomersInfos",
                schema: "CustomerManagement",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersInfos", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_CustomersInfos_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "CustomerManagement",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KycReviews",
                schema: "CustomerManagement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    KycCheckId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    KycLiveVideoId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Outcome = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KycReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KycReviews_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "CustomerManagement",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnboardCustomersAddresses",
                schema: "CustomerManagement",
                columns: table => new
                {
                    OnboardCustomerId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StreetLine = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardCustomersAddresses", x => x.OnboardCustomerId);
                    table.ForeignKey(
                        name: "FK_OnboardCustomersAddresses_OnboardCustomers_OnboardCustomerId",
                        column: x => x.OnboardCustomerId,
                        principalSchema: "CustomerManagement",
                        principalTable: "OnboardCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnboardCustomersInfos",
                schema: "CustomerManagement",
                columns: table => new
                {
                    OnboardCustomerId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardCustomersInfos", x => x.OnboardCustomerId);
                    table.ForeignKey(
                        name: "FK_OnboardCustomersInfos_OnboardCustomers_OnboardCustomerId",
                        column: x => x.OnboardCustomerId,
                        principalSchema: "CustomerManagement",
                        principalTable: "OnboardCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "CustomerManagement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    KycDocumentId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IssuingCountry = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    KycReviewId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Customers_Id",
                        column: x => x.Id,
                        principalSchema: "CustomerManagement",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_KycReviews_KycReviewId",
                        column: x => x.KycReviewId,
                        principalSchema: "CustomerManagement",
                        principalTable: "KycReviews",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocumentsAttachments",
                schema: "CustomerManagement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    KycAttachmentId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentsAttachments", x => new { x.DocumentId, x.Id });
                    table.ForeignKey(
                        name: "FK_DocumentsAttachments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "CustomerManagement",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                schema: "CustomerManagement",
                table: "Customers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_KycDocumentId",
                schema: "CustomerManagement",
                table: "Documents",
                column: "KycDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_KycReviewId",
                schema: "CustomerManagement",
                table: "Documents",
                column: "KycReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsAttachments_KycAttachmentId",
                schema: "CustomerManagement",
                table: "DocumentsAttachments",
                column: "KycAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_KycReviews_CustomerId",
                schema: "CustomerManagement",
                table: "KycReviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_KycReviews_KycCheckId",
                schema: "CustomerManagement",
                table: "KycReviews",
                column: "KycCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_KycReviews_KycLiveVideoId",
                schema: "CustomerManagement",
                table: "KycReviews",
                column: "KycLiveVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardCustomers_UserId",
                schema: "CustomerManagement",
                table: "OnboardCustomers",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "CustomerManagement");

            migrationBuilder.DropTable(
                name: "CustomersInfos",
                schema: "CustomerManagement");

            migrationBuilder.DropTable(
                name: "DocumentsAttachments",
                schema: "CustomerManagement");

            migrationBuilder.DropTable(
                name: "OnboardCustomersAddresses",
                schema: "CustomerManagement");

            migrationBuilder.DropTable(
                name: "OnboardCustomersInfos",
                schema: "CustomerManagement");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "CustomerManagement");

            migrationBuilder.DropTable(
                name: "OnboardCustomers",
                schema: "CustomerManagement");

            migrationBuilder.DropTable(
                name: "KycReviews",
                schema: "CustomerManagement");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "CustomerManagement");
        }
    }
}
