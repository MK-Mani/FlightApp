using Microsoft.EntityFrameworkCore.Migrations;

namespace ManageBookingAPIService.Migrations
{
    public partial class BookingMigrationIsBusinessClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBusinessClass",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReturnIsBusinessClass",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBusinessClass",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ReturnIsBusinessClass",
                table: "Bookings");
        }
    }
}
