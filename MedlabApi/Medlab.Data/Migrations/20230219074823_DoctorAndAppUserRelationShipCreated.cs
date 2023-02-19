using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medlab.Data.Migrations
{
    public partial class DoctorAndAppUserRelationShipCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_AppUserId",
                table: "Doctors",
                column: "AppUserId",
                unique: true,
                filter: "[AppUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AspNetUsers_AppUserId",
                table: "Doctors",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AspNetUsers_AppUserId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_AppUserId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "AspNetUsers");
        }
    }
}
