using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceClassification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ProductClassification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentClassificationId",
                table: "ProductClassification",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductClassification_ParentClassificationId",
                table: "ProductClassification",
                column: "ParentClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassification_ProductClassification_ParentClassificationId",
                table: "ProductClassification",
                column: "ParentClassificationId",
                principalTable: "ProductClassification",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassification_ProductClassification_ParentClassificationId",
                table: "ProductClassification");

            migrationBuilder.DropIndex(
                name: "IX_ProductClassification_ParentClassificationId",
                table: "ProductClassification");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ProductClassification");

            migrationBuilder.DropColumn(
                name: "ParentClassificationId",
                table: "ProductClassification");
        }
    }
}
