using Microsoft.EntityFrameworkCore.Migrations;

namespace Bcc.Pledg.Migrations
{
    public partial class AddBargainDeleteTypeEstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Corridor",
                table: "PLEDGE_PledgeRefs");

            migrationBuilder.DropColumn(
                name: "TypeEstate",
                table: "PLEDGE_PledgeRefs");

            migrationBuilder.DropColumn(
                name: "Corridor",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "TypeEstate",
                table: "PLEDGE_LogData");

            migrationBuilder.AddColumn<decimal>(
                name: "Bargain",
                table: "PLEDGE_PledgeRefs",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Bargain",
                table: "PLEDGE_LogData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bargain",
                table: "PLEDGE_PledgeRefs");

            migrationBuilder.DropColumn(
                name: "Bargain",
                table: "PLEDGE_LogData");

            migrationBuilder.AddColumn<decimal>(
                name: "Corridor",
                table: "PLEDGE_PledgeRefs",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TypeEstate",
                table: "PLEDGE_PledgeRefs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Corridor",
                table: "PLEDGE_LogData",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TypeEstate",
                table: "PLEDGE_LogData",
                type: "text",
                nullable: true);
        }
    }
}
