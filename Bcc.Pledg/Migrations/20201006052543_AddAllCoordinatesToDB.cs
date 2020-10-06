using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bcc.Pledg.Migrations
{
    public partial class AddAllCoordinatesToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<char>(
                name: "IsArch",
                table: "PLEDGE_LogData",
                nullable: false,
                oldClrType: typeof(char),
                oldType: "character(1)",
                oldDefaultValue: '0');

            migrationBuilder.CreateTable(
                name: "PLEDGE_SectorsCityDB",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLEDGE_SectorsCityDB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PLEDGE_SectorsDB",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Sector = table.Column<int>(nullable: false),
                    SectorsCityDBId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLEDGE_SectorsDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PLEDGE_SectorsDB_PLEDGE_SectorsCityDB_SectorsCityDBId",
                        column: x => x.SectorsCityDBId,
                        principalTable: "PLEDGE_SectorsCityDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PLEDGE_CoordinatesDB",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Lng = table.Column<double>(nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    SectorsDBId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLEDGE_CoordinatesDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PLEDGE_CoordinatesDB_PLEDGE_SectorsDB_SectorsDBId",
                        column: x => x.SectorsDBId,
                        principalTable: "PLEDGE_SectorsDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PLEDGE_CoordinatesDB_SectorsDBId",
                table: "PLEDGE_CoordinatesDB",
                column: "SectorsDBId");

            migrationBuilder.CreateIndex(
                name: "IX_PLEDGE_SectorsDB_SectorsCityDBId",
                table: "PLEDGE_SectorsDB",
                column: "SectorsCityDBId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PLEDGE_CoordinatesDB");

            migrationBuilder.DropTable(
                name: "PLEDGE_SectorsDB");

            migrationBuilder.DropTable(
                name: "PLEDGE_SectorsCityDB");

            migrationBuilder.AlterColumn<char>(
                name: "IsArch",
                table: "PLEDGE_LogData",
                type: "character(1)",
                nullable: false,
                defaultValue: '0',
                oldClrType: typeof(char));
        }
    }
}
