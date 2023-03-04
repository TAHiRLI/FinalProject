using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medlab.Data.Migrations
{
    public partial class DiscountPercentDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscoutPercent",
                table: "Products",
                type: "Decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DiscoutPercent",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,2)");
        }
    }
}
