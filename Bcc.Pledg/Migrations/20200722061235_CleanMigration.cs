using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bcc.Pledg.Migrations
{
    public partial class CleanMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PLEDGE_LogData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityCodeKATO = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    SectorCode = table.Column<string>(nullable: true),
                    Sector = table.Column<int>(nullable: false),
                    RelativityLocation = table.Column<string>(nullable: true),
                    SectorDescription = table.Column<string>(nullable: true),
                    TypeEstateCode = table.Column<string>(nullable: true),
                    TypeEstateByRef = table.Column<string>(nullable: true),
                    TypeEstate = table.Column<string>(nullable: true),
                    ApartmentLayoutCode = table.Column<string>(nullable: true),
                    ApartmentLayout = table.Column<string>(nullable: true),
                    WallMaterialCode = table.Column<int>(nullable: true),
                    WallMaterial = table.Column<string>(nullable: true),
                    DetailAreaCode = table.Column<string>(nullable: true),
                    DetailArea = table.Column<string>(nullable: true),
                    MinCostPerSQM = table.Column<int>(nullable: true),
                    MaxCostPerSQM = table.Column<int>(nullable: true),
                    Corridor = table.Column<decimal>(nullable: false),
                    MinCostWithBargain = table.Column<int>(nullable: true),
                    MaxCostWithBargain = table.Column<int>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLEDGE_LogData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PLEDGE_PledgeRefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityCodeKATO = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    SectorCode = table.Column<string>(nullable: true),
                    Sector = table.Column<int>(nullable: false),
                    RelativityLocation = table.Column<string>(nullable: true),
                    SectorDescription = table.Column<string>(nullable: true),
                    TypeEstateCode = table.Column<string>(nullable: true),
                    TypeEstateByRef = table.Column<string>(nullable: true),
                    TypeEstate = table.Column<string>(nullable: true),
                    ApartmentLayoutCode = table.Column<string>(nullable: true),
                    ApartmentLayout = table.Column<string>(nullable: true),
                    WallMaterialCode = table.Column<int>(nullable: true),
                    WallMaterial = table.Column<string>(nullable: true),
                    DetailAreaCode = table.Column<string>(nullable: true),
                    DetailArea = table.Column<string>(nullable: true),
                    MinCostPerSQM = table.Column<int>(nullable: true),
                    MaxCostPerSQM = table.Column<int>(nullable: true),
                    Corridor = table.Column<decimal>(nullable: false),
                    MinCostWithBargain = table.Column<int>(nullable: true),
                    MaxCostWithBargain = table.Column<int>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLEDGE_PledgeRefs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PLEDGE_LogData");

            migrationBuilder.DropTable(
                name: "PLEDGE_PledgeRefs");
        }
    }
}
