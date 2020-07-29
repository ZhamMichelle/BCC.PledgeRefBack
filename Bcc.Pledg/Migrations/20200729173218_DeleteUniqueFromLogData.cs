using Microsoft.EntityFrameworkCore.Migrations;

namespace Bcc.Pledg.Migrations
{
    public partial class DeleteUniqueFromLogData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PLEDGE_LogData_Code",
                table: "PLEDGE_LogData");

            migrationBuilder.CreateIndex(
                name: "IX_PLEDGE_LogData_Code",
                table: "PLEDGE_LogData",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PLEDGE_LogData_Code",
                table: "PLEDGE_LogData");

            migrationBuilder.CreateIndex(
                name: "IX_PLEDGE_LogData_Code",
                table: "PLEDGE_LogData",
                column: "Code",
                unique: true);
        }
    }
}
