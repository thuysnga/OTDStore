using Microsoft.EntityFrameworkCore.Migrations;

namespace OTDStore.Data.Migrations
{
    public partial class UpdateBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Brands",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Brands");
        }
    }
}
