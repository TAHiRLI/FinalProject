using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medlab.Data.Migrations
{
    public partial class ValuesTableisFeaturedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Values",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Values");
        }
    }
}
