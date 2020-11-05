using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bcc.Pledg.Migrations
{
    public partial class PrimHousing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActualAdress",
                table: "PLEDGE_LogData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinQualityLevel",
                table: "PLEDGE_LogData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinQualityLevelCode",
                table: "PLEDGE_LogData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RCName",
                table: "PLEDGE_LogData",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RCNameCode",
                table: "PLEDGE_LogData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PLEDGE_LogData",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PLEDGE_PrimaryPledgeRefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(nullable: true),
                    CityCodeKATO = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    RCNameCode = table.Column<int>(nullable: true),
                    RCName = table.Column<string>(nullable: true),
                    ActualAdress = table.Column<string>(nullable: true),
                    FinQualityLevelCode = table.Column<string>(nullable: true),
                    FinQualityLevel = table.Column<string>(nullable: true),
                    MinCostPerSQM = table.Column<int>(nullable: true),
                    MaxCostPerSQM = table.Column<int>(nullable: true),
                    BeginDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLEDGE_PrimaryPledgeRefs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PLEDGE_PrimaryPledgeRefs");

            migrationBuilder.DropColumn(
                name: "ActualAdress",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "FinQualityLevel",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "FinQualityLevelCode",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "RCName",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "RCNameCode",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PLEDGE_LogData");
        }
    }
}
