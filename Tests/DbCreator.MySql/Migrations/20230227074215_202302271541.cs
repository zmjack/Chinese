using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCreator.MySql.Migrations;

public partial class _202302271541 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Numerics");

        migrationBuilder.AlterColumn<string>(
            name: "TraditionalPinyin",
            table: "Words",
            type: "varchar(64)",
            maxLength: 64,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AlterColumn<string>(
            name: "Traditional",
            table: "Words",
            type: "varchar(64)",
            maxLength: 64,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AlterColumn<string>(
            name: "SimplifiedPinyin",
            table: "Words",
            type: "varchar(64)",
            maxLength: 64,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AlterColumn<string>(
            name: "Simplified",
            table: "Words",
            type: "varchar(64)",
            maxLength: 64,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AlterColumn<string>(
            name: "Traditional",
            table: "Chars",
            type: "varchar(64)",
            maxLength: 64,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AlterColumn<string>(
            name: "Simplified",
            table: "Chars",
            type: "varchar(64)",
            maxLength: 64,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AlterColumn<string>(
            name: "Pinyins",
            table: "Chars",
            type: "varchar(64)",
            maxLength: 64,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AlterColumn<string>(
            name: "DefaultPinyin",
            table: "Chars",
            type: "varchar(64)",
            maxLength: 64,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Words",
            keyColumn: "TraditionalPinyin",
            keyValue: null,
            column: "TraditionalPinyin",
            value: "");

        migrationBuilder.AlterColumn<string>(
            name: "TraditionalPinyin",
            table: "Words",
            type: "varchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64,
            oldNullable: true)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.UpdateData(
            table: "Words",
            keyColumn: "Traditional",
            keyValue: null,
            column: "Traditional",
            value: "");

        migrationBuilder.AlterColumn<string>(
            name: "Traditional",
            table: "Words",
            type: "varchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64,
            oldNullable: true)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.UpdateData(
            table: "Words",
            keyColumn: "SimplifiedPinyin",
            keyValue: null,
            column: "SimplifiedPinyin",
            value: "");

        migrationBuilder.AlterColumn<string>(
            name: "SimplifiedPinyin",
            table: "Words",
            type: "varchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64,
            oldNullable: true)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.UpdateData(
            table: "Words",
            keyColumn: "Simplified",
            keyValue: null,
            column: "Simplified",
            value: "");

        migrationBuilder.AlterColumn<string>(
            name: "Simplified",
            table: "Words",
            type: "varchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64,
            oldNullable: true)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.UpdateData(
            table: "Chars",
            keyColumn: "Traditional",
            keyValue: null,
            column: "Traditional",
            value: "");

        migrationBuilder.AlterColumn<string>(
            name: "Traditional",
            table: "Chars",
            type: "varchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64,
            oldNullable: true)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.UpdateData(
            table: "Chars",
            keyColumn: "Simplified",
            keyValue: null,
            column: "Simplified",
            value: "");

        migrationBuilder.AlterColumn<string>(
            name: "Simplified",
            table: "Chars",
            type: "varchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64,
            oldNullable: true)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.UpdateData(
            table: "Chars",
            keyColumn: "Pinyins",
            keyValue: null,
            column: "Pinyins",
            value: "");

        migrationBuilder.AlterColumn<string>(
            name: "Pinyins",
            table: "Chars",
            type: "varchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64,
            oldNullable: true)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.UpdateData(
            table: "Chars",
            keyColumn: "DefaultPinyin",
            keyValue: null,
            column: "DefaultPinyin",
            value: "");

        migrationBuilder.AlterColumn<string>(
            name: "DefaultPinyin",
            table: "Chars",
            type: "varchar(64)",
            maxLength: 64,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64,
            oldNullable: true)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Numerics",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                Simplified = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                SimplifiedPinyin = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Traditional = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                TraditionalPinyin = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Type = table.Column<int>(type: "int", nullable: false),
                Value = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Numerics", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");
    }
}
