using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateShowSeatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShowSeats",
                columns: table => new
                {
                    ShowSeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowSeats", x => x.ShowSeatId);
                    table.ForeignKey(
                        name: "FK_ShowSeats_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "SeatId");
                    table.ForeignKey(
                        name: "FK_ShowSeats_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "ShowId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShowSeats_SeatId",
                table: "ShowSeats",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowSeats_ShowId_SeatId",
                table: "ShowSeats",
                columns: new[] { "ShowId", "SeatId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShowSeats");
        }
    }
}
