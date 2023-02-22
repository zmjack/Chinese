using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCreator.MySql.Migrations
{
    public partial class _202302211120 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Simplified",
                table: "Chars",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Traditional",
                table: "Chars",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Simplified",
                table: "Chars");

            migrationBuilder.DropColumn(
                name: "Traditional",
                table: "Chars");
        }
    }
}
