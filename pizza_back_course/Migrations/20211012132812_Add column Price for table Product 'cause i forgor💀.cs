using Microsoft.EntityFrameworkCore.Migrations;

namespace pizza_back_course.Migrations
{
    public partial class AddcolumnPricefortableProductcauseiforgor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "products");
        }
    }
}
