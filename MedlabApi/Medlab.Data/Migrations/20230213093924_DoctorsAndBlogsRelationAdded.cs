using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Medlab.Data.Migrations
{
    public partial class DoctorsAndBlogsRelationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_DoctorId",
                table: "Blogs",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Doctors_DoctorId",
                table: "Blogs",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Doctors_DoctorId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_DoctorId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Blogs");
        }
    }
}
