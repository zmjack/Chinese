using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCreator.MySql.Migrations;

public partial class _202302210832 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "HashCode",
            table: "Words",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "HashCode",
            table: "Words");
    }
}
