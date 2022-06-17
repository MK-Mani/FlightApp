using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManageBookingAPIService.Migrations
{
    public partial class FlightDbMaigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    AirlineRecId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirlineName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AirlineLogo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ContactAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DiscountCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiscountAmount = table.Column<double>(type: "float", nullable: true),
                    IsBlock = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.AirlineRecId);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleRecId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirlineRecId = table.Column<int>(type: "int", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FromPlace = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ToPlace = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleDays = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InstrumentUsed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NoOfBussinessClsSeats = table.Column<int>(type: "int", nullable: false),
                    NoOfNonBussinessClsSeats = table.Column<int>(type: "int", nullable: false),
                    BusinessClassTicket = table.Column<double>(type: "float", nullable: false),
                    NonBusinessClassTicket = table.Column<double>(type: "float", nullable: false),
                    NoOfRows = table.Column<int>(type: "int", nullable: false),
                    Meal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleRecId);
                    table.ForeignKey(
                        name: "FK_Schedules_Airlines_AirlineRecId",
                        column: x => x.AirlineRecId,
                        principalTable: "Airlines",
                        principalColumn: "AirlineRecId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingRecId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleRecId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NoOfSeats = table.Column<int>(type: "int", nullable: false),
                    Meal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PNRNumber = table.Column<int>(type: "int", nullable: false),
                    DateOfJourney = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRoundTrip = table.Column<bool>(type: "bit", nullable: false),
                    ReturnScheduleRecId = table.Column<int>(type: "int", nullable: true),
                    ReturnDateOfJourney = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCancelTicket = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingRecId);
                    table.ForeignKey(
                        name: "FK_Bookings_Schedules_ScheduleRecId",
                        column: x => x.ScheduleRecId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleRecId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passenger",
                columns: table => new
                {
                    PassengerRecId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingRecId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SeatNumbers = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ReturnSeatNumbers = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passenger", x => x.PassengerRecId);
                    table.ForeignKey(
                        name: "FK_Passenger_Bookings_BookingRecId",
                        column: x => x.BookingRecId,
                        principalTable: "Bookings",
                        principalColumn: "BookingRecId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ScheduleRecId",
                table: "Bookings",
                column: "ScheduleRecId");

            migrationBuilder.CreateIndex(
                name: "IX_Passenger_BookingRecId",
                table: "Passenger",
                column: "BookingRecId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_AirlineRecId",
                table: "Schedules",
                column: "AirlineRecId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Passenger");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Airlines");
        }
    }
}
