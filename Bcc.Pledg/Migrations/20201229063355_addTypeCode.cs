using Microsoft.EntityFrameworkCore.Migrations;

namespace Bcc.Pledg.Migrations
{
    public partial class addTypeCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<char>(
                name: "TypeCode",
                table: "PLEDGE_LogData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeCode",
                table: "PLEDGE_LogData");
        }
    }
}
