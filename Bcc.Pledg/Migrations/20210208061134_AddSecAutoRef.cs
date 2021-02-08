using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bcc.Pledg.Migrations
{
    public partial class AddSecAutoRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarBrand",
                table: "PLEDGE_LogData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarModel",
                table: "PLEDGE_LogData",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MarketCost",
                table: "PLEDGE_LogData",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "MaxPercentageDeviation",
                table: "PLEDGE_LogData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProduceYesr",
                table: "PLEDGE_LogData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PLEDGE_SecondaryAutoRefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(nullable: true),
                    CarBrand = table.Column<string>(nullable: true),
                    CarModel = table.Column<string>(nullable: true),
                    ProduceYesr = table.Column<int>(nullable: false),
                    MarketCost = table.Column<long>(nullable: false),
                    MaxPercentageDeviation = table.Column<int>(nullable: false),
                    BeginDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLEDGE_SecondaryAutoRefs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PLEDGE_SecondaryAutoRefs");

            migrationBuilder.DropColumn(
                name: "CarBrand",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "CarModel",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "MarketCost",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "MaxPercentageDeviation",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "ProduceYesr",
                table: "PLEDGE_LogData");
        }
    }
}
