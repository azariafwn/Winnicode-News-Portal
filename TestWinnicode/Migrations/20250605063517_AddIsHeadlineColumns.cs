using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestWinnicode.Migrations
{
    /// <inheritdoc />
    public partial class AddIsHeadlineColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Penulis",
                table: "Berita");

            migrationBuilder.CreateTable(
                name: "Penulis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Deskripsi = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FotoProfil = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalArtikel = table.Column<int>(type: "int", nullable: false),
                    TotalDibaca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penulis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penulis_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Berita_PenulisId",
                table: "Berita",
                column: "PenulisId");

            migrationBuilder.CreateIndex(
                name: "IX_Penulis_UserId",
                table: "Penulis",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Berita_Penulis_PenulisId",
                table: "Berita",
                column: "PenulisId",
                principalTable: "Penulis",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Berita_Penulis_PenulisId",
                table: "Berita");

            migrationBuilder.DropTable(
                name: "Penulis");

            migrationBuilder.DropIndex(
                name: "IX_Berita_PenulisId",
                table: "Berita");

            migrationBuilder.AddColumn<string>(
                name: "Penulis",
                table: "Berita",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
