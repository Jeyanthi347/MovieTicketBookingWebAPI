using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRatingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: true),
                    TheatreId = table.Column<int>(type: "int", nullable: true),
                    RatingValue = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: false),
                    Review = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.RatingId);
                    table.CheckConstraint("CK_Ratings_OneTarget", "([MovieId] IS NOT NULL AND [TheatreId] IS NULL) OR ([MovieId] IS NULL AND [TheatreId] IS NOT NULL)");
                    table.CheckConstraint("CK_Ratings_RatingValue", "[RatingValue] >= 1 AND [RatingValue] <= 10");
                    table.ForeignKey(
                        name: "FK_Ratings_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId");
                    table.ForeignKey(
                        name: "FK_Ratings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId");
                    table.ForeignKey(
                        name: "FK_Ratings_Theatres_TheatreId",
                        column: x => x.TheatreId,
                        principalTable: "Theatres",
                        principalColumn: "TheatreId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_BookingId_MovieId",
                table: "Ratings",
                columns: new[] { "BookingId", "MovieId" },
                unique: true,
                filter: "[MovieId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_BookingId_TheatreId",
                table: "Ratings",
                columns: new[] { "BookingId", "TheatreId" },
                unique: true,
                filter: "[TheatreId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MovieId",
                table: "Ratings",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_TheatreId",
                table: "Ratings",
                column: "TheatreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ratings");
        }
    }
}
