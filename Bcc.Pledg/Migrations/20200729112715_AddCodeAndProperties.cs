using Microsoft.EntityFrameworkCore.Migrations;

namespace Bcc.Pledg.Migrations
{
    public partial class AddCodeAndProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousId",
                table: "PLEDGE_LogData");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PLEDGE_PledgeRefs",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PLEDGE_LogData",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PLEDGE_PledgeRefs_City",
                table: "PLEDGE_PledgeRefs",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_PLEDGE_PledgeRefs_Code",
                table: "PLEDGE_PledgeRefs",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PLEDGE_PledgeRefs_Sector",
                table: "PLEDGE_PledgeRefs",
                column: "Sector");

            migrationBuilder.CreateIndex(
                name: "IX_PLEDGE_PledgeRefs_TypeEstateByRef",
                table: "PLEDGE_PledgeRefs",
                column: "TypeEstateByRef");

            migrationBuilder.CreateIndex(
                name: "IX_PLEDGE_LogData_Code",
                table: "PLEDGE_LogData",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PLEDGE_PledgeRefs_City",
                table: "PLEDGE_PledgeRefs");

            migrationBuilder.DropIndex(
                name: "IX_PLEDGE_PledgeRefs_Code",
                table: "PLEDGE_PledgeRefs");

            migrationBuilder.DropIndex(
                name: "IX_PLEDGE_PledgeRefs_Sector",
                table: "PLEDGE_PledgeRefs");

            migrationBuilder.DropIndex(
                name: "IX_PLEDGE_PledgeRefs_TypeEstateByRef",
                table: "PLEDGE_PledgeRefs");

            migrationBuilder.DropIndex(
                name: "IX_PLEDGE_LogData_Code",
                table: "PLEDGE_LogData");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PLEDGE_PledgeRefs");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PLEDGE_LogData");

            migrationBuilder.AddColumn<int>(
                name: "PreviousId",
                table: "PLEDGE_LogData",
                type: "integer",
                nullable: true);
        }
    }
}
