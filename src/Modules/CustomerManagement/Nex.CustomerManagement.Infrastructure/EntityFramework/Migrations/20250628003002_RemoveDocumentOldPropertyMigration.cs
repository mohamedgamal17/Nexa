using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDocumentOldPropertyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Document_UserId",
                schema: "CustomerManagement",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "CustomerManagement",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                schema: "CustomerManagement",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "CustomerManagement",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "CustomerManagement",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                schema: "CustomerManagement",
                table: "Document");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "CustomerManagement",
                table: "Document",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                schema: "CustomerManagement",
                table: "Document",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "CustomerManagement",
                table: "Document",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "CustomerManagement",
                table: "Document",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                schema: "CustomerManagement",
                table: "Document",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Document_UserId",
                schema: "CustomerManagement",
                table: "Document",
                column: "UserId");
        }
    }
}
