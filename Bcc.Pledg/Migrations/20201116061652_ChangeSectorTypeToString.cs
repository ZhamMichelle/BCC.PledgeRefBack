using Microsoft.EntityFrameworkCore.Migrations;

namespace Bcc.Pledg.Migrations
{
    public partial class ChangeSectorTypeToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Sector",
                table: "PLEDGE_SectorsDB",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Sector",
                table: "PLEDGE_PledgeRefs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Sector",
                table: "PLEDGE_LogData",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sector",
                table: "PLEDGE_SectorsDB",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Sector",
                table: "PLEDGE_PledgeRefs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Sector",
                table: "PLEDGE_LogData",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
