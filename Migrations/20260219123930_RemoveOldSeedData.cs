using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantAB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "MenuId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "MenuId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "MenuId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "MenuId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "MenuId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "MenuId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableId",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CustomerEmail", "CustomerName", "CustomerPhone" },
                values: new object[,]
                {
                    { 1, "anna@example.com", "Anna Svensson", "0701234567" },
                    { 2, "erik@example.com", "Erik Johansson", "0707654321" }
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "MenuId", "Description", "ImageUrl", "IsPopular", "Name", "Price" },
                values: new object[,]
                {
                    { 1, null, null, false, "Pizza Margherita", 95.00m },
                    { 2, null, null, false, "Pasta Carbonara", 110.00m },
                    { 3, null, null, false, "Caesar Salad", 85.00m },
                    { 4, null, null, true, "Kebab", 95.00m },
                    { 5, null, null, true, "Banana split", 110.00m },
                    { 6, null, null, true, "Pie", 85.00m }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "TableId", "Capacity", "TableNumber" },
                values: new object[,]
                {
                    { 1, 2, 0 },
                    { 2, 4, 0 },
                    { 3, 6, 0 }
                });
        }
    }
}
