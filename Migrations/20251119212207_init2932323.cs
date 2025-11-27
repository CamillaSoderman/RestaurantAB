using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAB.Migrations
{
    /// <inheritdoc />
    public partial class init2932323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$uVhX9YwVhQ9JH7oXzFqZkO9uQkYwVhQ9JH7oXzFqZkUuYz7QhZVhZ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$9uQkYwVhQ9JH7oXzFqZkUuYz7QhZVhZkzFfZkzYwqk5JcFhQxvZp1u");
        }
    }
}
