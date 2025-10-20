using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RemoveObsoletePropertiesAndUpdateTheExistMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomersInfos_KycReviews_KycReviewId",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.DropIndex(
                name: "IX_CustomersInfos_KycReviewId",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.DropColumn(
                name: "IdNumber",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.DropColumn(
                name: "KycReviewId",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.DropColumn(
                name: "Nationality",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "CustomerManagement",
                table: "CustomersInfos");

            migrationBuilder.RenameColumn(
                name: "State",
                schema: "CustomerManagement",
                table: "Documents",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "State",
                schema: "CustomerManagement",
                table: "Customers",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "CustomerManagement",
                table: "Documents",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "CustomerManagement",
                table: "Customers",
                newName: "State");

            migrationBuilder.AddColumn<string>(
                name: "IdNumber",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KycReviewId",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomersInfos_KycReviewId",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                column: "KycReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersInfos_KycReviews_KycReviewId",
                schema: "CustomerManagement",
                table: "CustomersInfos",
                column: "KycReviewId",
                principalSchema: "CustomerManagement",
                principalTable: "KycReviews",
                principalColumn: "Id");
        }
    }
}
