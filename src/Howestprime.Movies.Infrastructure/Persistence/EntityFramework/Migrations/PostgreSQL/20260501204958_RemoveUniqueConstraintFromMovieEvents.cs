using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Migrations.PostgreSQL
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstraintFromMovieEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MovieEvents_RoomId_ShowTime",
                table: "MovieEvents");

            migrationBuilder.CreateIndex(
                name: "IX_MovieEvents_RoomId_ShowTime",
                table: "MovieEvents",
                columns: new[] { "RoomId", "ShowTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MovieEvents_RoomId_ShowTime",
                table: "MovieEvents");

            migrationBuilder.CreateIndex(
                name: "IX_MovieEvents_RoomId_ShowTime",
                table: "MovieEvents",
                columns: new[] { "RoomId", "ShowTime" },
                unique: true);
        }
    }
}
