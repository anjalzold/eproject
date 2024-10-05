using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eproject.Migrations
{
    /// <inheritdoc />
    public partial class dddddAddDynamicAnimeProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnimeStatus",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Studio",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Views",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimeStatus",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Studio",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Animes");
        }
    }
}
