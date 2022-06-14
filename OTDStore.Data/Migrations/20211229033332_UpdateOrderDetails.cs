using Microsoft.EntityFrameworkCore.Migrations;

namespace OTDStore.Data.Migrations
{
    public partial class UpdateOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "OrderDetails",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Memory",
                table: "OrderDetails",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderDetails",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RAM",
                table: "OrderDetails",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Memory",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "RAM",
                table: "OrderDetails");
        }
    }
}
