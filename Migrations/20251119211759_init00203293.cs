using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAB.Migrations
{
    /// <inheritdoc />
    public partial class init00203293 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[] { 2, "admin@example.se", "$2a$11$9uQkYwVhQ9JH7oXzFqZkUuYz7QhZVhZkzFfZkzYwqk5JcFhQxvZp1u", "Admin", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, "admin@example.com", "$2a$11$QeYkYwVhQ9JH7oXzFqZkUuYz7QhZVhZkzFfZkzYwqk5JcFhQxvZp1u", "Admin", "admin" });
        }
    }
}
