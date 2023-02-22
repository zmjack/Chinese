using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCreator.MySql.Migrations
{
    public partial class _202302210846 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultPinyin",
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
                name: "DefaultPinyin",
                table: "Chars");
        }
    }
}
