using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bcc.Pledg.Migrations
{
    public partial class addLogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ChangeDate",
                table: "PLEDGE_LogData",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PreviousId",
                table: "PLEDGE_LogData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeDate",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "PreviousId",
                table: "PLEDGE_LogData");
        }
    }
}
