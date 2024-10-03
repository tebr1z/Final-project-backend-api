using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LmsApiApp.DataAccess.Migrations
{
    public partial class CourseAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Mevcut Foreign Key'i yeniden tanımlamıyoruz
            // migrationBuilder.AddForeignKey(
            //     name: "FK_Courses_AspNetUsers_UserId",
            //     table: "Courses",
            //     column: "UserId",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);

            // GroupId alanını ekliyoruz
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_GroupId",
                table: "Courses",
                column: "GroupId");

            // Group ile Foreign Key ilişkisini ekliyoruz
            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Groups_GroupId",
                table: "Courses",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Groups_GroupId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_GroupId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Courses");

            // Aşağıdaki Foreign Key kaldırmayı yeniden eklemeye gerek yok
            // migrationBuilder.AddForeignKey(
            //     name: "FK_Courses_AspNetUsers_UserId",
            //     table: "Courses",
            //     column: "UserId",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id");
        }
    }
}
