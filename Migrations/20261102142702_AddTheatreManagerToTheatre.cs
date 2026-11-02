using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTheatreManagerToTheatre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TheatreManagerUserId",
                table: "Theatres",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Theatres_TheatreManagerUserId",
                table: "Theatres",
                column: "TheatreManagerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Theatres_Users_TheatreManagerUserId",
                table: "Theatres",
                column: "TheatreManagerUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Theatres_Users_TheatreManagerUserId",
                table: "Theatres");

            migrationBuilder.DropIndex(
                name: "IX_Theatres_TheatreManagerUserId",
                table: "Theatres");

            migrationBuilder.DropColumn(
                name: "TheatreManagerUserId",
                table: "Theatres");
        }
    }
}
