using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EersteWebNetApplicatie.Migrations
{
    /// <inheritdoc />
    public partial class EersteMigratie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogposts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    inhoud = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    publicatieDatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogposts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    beschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GitHubUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogposts");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
