using Microsoft.EntityFrameworkCore.Migrations;

namespace Bcc.Pledg.Migrations
{
    public partial class AddIsArch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<char>(
                name: "IsArch",
                table: "PLEDGE_LogData",
                nullable: false,
                defaultValue: '0');
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArch",
                table: "PLEDGE_LogData");
        }
    }
}
