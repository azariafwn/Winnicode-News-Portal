using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestWinnicode.Migrations
{
    /// <inheritdoc />
    public partial class AddHeadlineFieldsToBerita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHeadline",
                table: "Kategori",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubHeadline",
                table: "Kategori",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHeadline",
                table: "Berita",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubHeadline",
                table: "Berita",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHeadline",
                table: "Kategori");

            migrationBuilder.DropColumn(
                name: "IsSubHeadline",
                table: "Kategori");

            migrationBuilder.DropColumn(
                name: "IsHeadline",
                table: "Berita");

            migrationBuilder.DropColumn(
                name: "IsSubHeadline",
                table: "Berita");
        }
    }
}
