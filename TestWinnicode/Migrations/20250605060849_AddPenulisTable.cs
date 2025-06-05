using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestWinnicode.Migrations
{
    /// <inheritdoc />
    public partial class AddPenulisTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PenulisId",
                table: "Berita",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PenulisId",
                table: "Berita");
        }
    }
}
