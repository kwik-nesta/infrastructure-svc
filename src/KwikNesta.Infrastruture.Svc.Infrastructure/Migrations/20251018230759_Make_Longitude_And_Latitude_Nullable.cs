using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KwikNesta.Infrastruture.Svc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Make_Longitude_And_Latitude_Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "infrastructure-svc");

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                schema: "infrastructure-svc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Domain = table.Column<int>(type: "integer", nullable: false),
                    DomainId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    PerformedBy = table.Column<string>(type: "text", nullable: false),
                    TargetId = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "infrastructure-svc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ISO2 = table.Column<string>(type: "text", nullable: false),
                    ISO3 = table.Column<string>(type: "text", nullable: false),
                    NumericCode = table.Column<string>(type: "text", nullable: false),
                    PhoneCode = table.Column<string>(type: "text", nullable: false),
                    Capital = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    CurrencyName = table.Column<string>(type: "text", nullable: false),
                    CurrencySymbol = table.Column<string>(type: "text", nullable: false),
                    TLD = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    SubRegion = table.Column<string>(type: "text", nullable: false),
                    Native = table.Column<string>(type: "text", nullable: false),
                    Nationality = table.Column<string>(type: "text", nullable: false),
                    Longitude = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<string>(type: "text", nullable: true),
                    Emoji = table.Column<string>(type: "text", nullable: false),
                    EmojiUnicode = table.Column<string>(type: "text", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                schema: "infrastructure-svc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    ISO2 = table.Column<string>(type: "text", nullable: false),
                    Longitude = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "infrastructure-svc",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeZones",
                schema: "infrastructure-svc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ZoneName = table.Column<string>(type: "text", nullable: false),
                    GMTOffset = table.Column<long>(type: "bigint", nullable: false),
                    GMTOffsetName = table.Column<string>(type: "text", nullable: false),
                    Abbreviation = table.Column<string>(type: "text", nullable: false),
                    TZName = table.Column<string>(type: "text", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeZones_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "infrastructure-svc",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "infrastructure-svc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Longitude = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    StateId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "infrastructure-svc",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalSchema: "infrastructure-svc",
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                schema: "infrastructure-svc",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                schema: "infrastructure-svc",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                schema: "infrastructure-svc",
                table: "States",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeZones_CountryId",
                schema: "infrastructure-svc",
                table: "TimeZones",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditTrails",
                schema: "infrastructure-svc");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "infrastructure-svc");

            migrationBuilder.DropTable(
                name: "TimeZones",
                schema: "infrastructure-svc");

            migrationBuilder.DropTable(
                name: "States",
                schema: "infrastructure-svc");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "infrastructure-svc");
        }
    }
}
