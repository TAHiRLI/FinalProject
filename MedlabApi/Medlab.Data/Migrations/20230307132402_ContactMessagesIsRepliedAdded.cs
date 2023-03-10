using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medlab.Data.Migrations
{
    public partial class ContactMessagesIsRepliedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReplied",
                table: "ContactMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReplied",
                table: "ContactMessages");
        }
    }
}
