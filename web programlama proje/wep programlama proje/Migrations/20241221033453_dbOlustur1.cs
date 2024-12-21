using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wep_programlama_proje.Migrations
{
    /// <inheritdoc />
    public partial class dbOlustur1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Maas",
                table: "Calisanlar");

            migrationBuilder.RenameColumn(
                name: "Uzmanlik",
                table: "Calisanlar",
                newName: "CalisanUzmanlik");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Calisanlar",
                newName: "CalisanId");

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    MusteiId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MusteriAd = table.Column<string>(type: "TEXT", nullable: false),
                    MusteriEmail = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.MusteiId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Musteriler");

            migrationBuilder.RenameColumn(
                name: "CalisanUzmanlik",
                table: "Calisanlar",
                newName: "Uzmanlik");

            migrationBuilder.RenameColumn(
                name: "CalisanId",
                table: "Calisanlar",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "Maas",
                table: "Calisanlar",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
