using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedClassifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassification_ProductClassification_ParentClassificationId",
                table: "ProductClassification");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductClassification_ProductClassificationId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductClassification",
                table: "ProductClassification");

            migrationBuilder.RenameTable(
                name: "ProductClassification",
                newName: "ProductClassifications");

            migrationBuilder.RenameIndex(
                name: "IX_ProductClassification_ParentClassificationId",
                table: "ProductClassifications",
                newName: "IX_ProductClassifications_ParentClassificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductClassifications",
                table: "ProductClassifications",
                column: "Id");

            migrationBuilder.InsertData(
                table: "ProductClassifications",
                columns: new[] { "Id", "Code", "Name", "ParentClassificationId" },
                values: new object[,]
                {
                    { 1, "ELEC", "Electronics", null },
                    { 4, "GROC", "Groceries", null },
                    { 2, "MOB", "Mobile Phones", 1 },
                    { 3, "LAP", "Laptops", 1 },
                    { 5, "BEV", "Beverages", 4 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassifications_ProductClassifications_ParentClassificationId",
                table: "ProductClassifications",
                column: "ParentClassificationId",
                principalTable: "ProductClassifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductClassifications_ProductClassificationId",
                table: "Products",
                column: "ProductClassificationId",
                principalTable: "ProductClassifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductClassifications_ProductClassifications_ParentClassificationId",
                table: "ProductClassifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductClassifications_ProductClassificationId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductClassifications",
                table: "ProductClassifications");

            migrationBuilder.DeleteData(
                table: "ProductClassifications",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductClassifications",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductClassifications",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductClassifications",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductClassifications",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.RenameTable(
                name: "ProductClassifications",
                newName: "ProductClassification");

            migrationBuilder.RenameIndex(
                name: "IX_ProductClassifications_ParentClassificationId",
                table: "ProductClassification",
                newName: "IX_ProductClassification_ParentClassificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductClassification",
                table: "ProductClassification",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductClassification_ProductClassification_ParentClassificationId",
                table: "ProductClassification",
                column: "ParentClassificationId",
                principalTable: "ProductClassification",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductClassification_ProductClassificationId",
                table: "Products",
                column: "ProductClassificationId",
                principalTable: "ProductClassification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
