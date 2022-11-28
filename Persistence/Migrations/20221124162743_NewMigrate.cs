using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class NewMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CastTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CastTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CoverUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReleaseDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfileImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TagNames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagNames", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProfileImageUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CastEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Role = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PersonId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    TypeId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FilmId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CastEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CastEntries_CastTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CastTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CastEntries_Movies_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CastEntries_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TagEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TagId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FilmId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagEntries_Movies_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TagEntries_TagNames_TagId",
                        column: x => x.TagId,
                        principalTable: "TagNames",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FavoriteEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FilmId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FanId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteEntries_Movies_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Movies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavoriteEntries_Users_FanId",
                        column: x => x.FanId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CastEntries_FilmId",
                table: "CastEntries",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_CastEntries_PersonId",
                table: "CastEntries",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CastEntries_TypeId",
                table: "CastEntries",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteEntries_FanId",
                table: "FavoriteEntries",
                column: "FanId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteEntries_FilmId",
                table: "FavoriteEntries",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_TagEntries_FilmId",
                table: "TagEntries",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_TagEntries_TagId",
                table: "TagEntries",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CastEntries");

            migrationBuilder.DropTable(
                name: "FavoriteEntries");

            migrationBuilder.DropTable(
                name: "TagEntries");

            migrationBuilder.DropTable(
                name: "CastTypes");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "TagNames");
        }
    }
}
