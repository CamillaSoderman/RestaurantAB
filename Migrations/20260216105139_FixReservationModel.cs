using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAB.Migrations
{
    /// <inheritdoc />
    public partial class FixReservationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessCode",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reservations",
                newName: "ResId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResId",
                table: "Reservations",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "AccessCode",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
