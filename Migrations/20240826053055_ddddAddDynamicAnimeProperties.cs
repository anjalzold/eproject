using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eproject.Migrations
{
    /// <inheritdoc />
    public partial class ddddAddDynamicAnimeProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimeStatus",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Studios",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Animes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnimeStatus",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Studios",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Views",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
