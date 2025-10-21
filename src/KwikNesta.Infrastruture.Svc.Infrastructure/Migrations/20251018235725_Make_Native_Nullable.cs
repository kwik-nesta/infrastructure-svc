using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KwikNesta.Infrastruture.Svc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Make_Native_Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GMTOffsetName",
                schema: "infrastructure-svc",
                table: "TimeZones",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Abbreviation",
                schema: "infrastructure-svc",
                table: "TimeZones",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Native",
                schema: "infrastructure-svc",
                table: "Countries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GMTOffsetName",
                schema: "infrastructure-svc",
                table: "TimeZones",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Abbreviation",
                schema: "infrastructure-svc",
                table: "TimeZones",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Native",
                schema: "infrastructure-svc",
                table: "Countries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
