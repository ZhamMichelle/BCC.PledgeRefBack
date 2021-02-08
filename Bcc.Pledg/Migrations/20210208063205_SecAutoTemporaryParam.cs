using Microsoft.EntityFrameworkCore.Migrations;

namespace Bcc.Pledg.Migrations
{
    public partial class SecAutoTemporaryParam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProduceYesr",
                table: "PLEDGE_SecondaryAutoRefs");

            migrationBuilder.DropColumn(
                name: "ProduceYesr",
                table: "PLEDGE_LogData");

            migrationBuilder.AlterColumn<int>(
                name: "MaxPercentageDeviation",
                table: "PLEDGE_SecondaryAutoRefs",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "MarketCost",
                table: "PLEDGE_SecondaryAutoRefs",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "ProduceYear",
                table: "PLEDGE_SecondaryAutoRefs",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxPercentageDeviation",
                table: "PLEDGE_LogData",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "MarketCost",
                table: "PLEDGE_LogData",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "ProduceYear",
                table: "PLEDGE_LogData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProduceYear",
                table: "PLEDGE_SecondaryAutoRefs");

            migrationBuilder.DropColumn(
                name: "ProduceYear",
                table: "PLEDGE_LogData");

            migrationBuilder.AlterColumn<int>(
                name: "MaxPercentageDeviation",
                table: "PLEDGE_SecondaryAutoRefs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MarketCost",
                table: "PLEDGE_SecondaryAutoRefs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProduceYesr",
                table: "PLEDGE_SecondaryAutoRefs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MaxPercentageDeviation",
                table: "PLEDGE_LogData",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MarketCost",
                table: "PLEDGE_LogData",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProduceYesr",
                table: "PLEDGE_LogData",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
