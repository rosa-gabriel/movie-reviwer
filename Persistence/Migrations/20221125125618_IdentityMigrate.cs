using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class IdentityMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteEntries_Users_FanId",
                table: "FavoriteEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteEntries_FanId",
                table: "FavoriteEntries");

            migrationBuilder.DropColumn(
                name: "FanId",
                table: "FavoriteEntries");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "SecurityStamp");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ProfileImageUrl",
                keyValue: null,
                column: "ProfileImageUrl",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileImageUrl",
                table: "Users",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FanProfileImageUrl",
                table: "FavoriteEntries",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "ProfileImageUrl");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteEntries_FanProfileImageUrl",
                table: "FavoriteEntries",
                column: "FanProfileImageUrl");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteEntries_Users_FanProfileImageUrl",
                table: "FavoriteEntries",
                column: "FanProfileImageUrl",
                principalTable: "Users",
                principalColumn: "ProfileImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteEntries_Users_FanProfileImageUrl",
                table: "FavoriteEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteEntries_FanProfileImageUrl",
                table: "FavoriteEntries");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FanProfileImageUrl",
                table: "FavoriteEntries");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                table: "Users",
                newName: "Name");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileImageUrl",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "FanId",
                table: "FavoriteEntries",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteEntries_FanId",
                table: "FavoriteEntries",
                column: "FanId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteEntries_Users_FanId",
                table: "FavoriteEntries",
                column: "FanId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
