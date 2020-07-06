using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BCC.PledgeRefBack.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PledgeRefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CityCodeKATO = table.Column<int>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    SectorCode = table.Column<string>(nullable: true),
                    Sector = table.Column<int>(nullable: false),
                    RelativityLocation = table.Column<string>(nullable: true),
                    SectorDescription = table.Column<string>(nullable: true),
                    TypeEstateCode = table.Column<string>(nullable: true),
                    TypeEstateByRef = table.Column<string>(nullable: true),
                    TypeEstate = table.Column<string>(nullable: true),
                    ApartmentLayoutCode = table.Column<string>(nullable: true),
                    ApartmentLayout = table.Column<string>(nullable: true),
                    WallMaterialCode = table.Column<int>(nullable: false),
                    WallMaterial = table.Column<string>(nullable: true),
                    DetailAreaCode = table.Column<string>(nullable: true),
                    DetailArea = table.Column<string>(nullable: true),
                    MinCostPerSQM = table.Column<int>(nullable: false),
                    MaxCostPerSQM = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PledgeRefs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PledgeRefs");
        }
    }
}
