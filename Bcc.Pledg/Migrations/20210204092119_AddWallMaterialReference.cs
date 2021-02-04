using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bcc.Pledg.Migrations
{
    public partial class AddWallMaterialReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PLEDGE_WallMaterialReferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    WallMaterialCodeColvir = table.Column<int>(nullable: true),
                    WallMaterialColvir = table.Column<string>(nullable: true),
                    WallMaterialCodeGF = table.Column<int>(nullable: true),
                    WallMaterialGF = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLEDGE_WallMaterialReferences", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PLEDGE_WallMaterialReferences");
        }
    }
}
