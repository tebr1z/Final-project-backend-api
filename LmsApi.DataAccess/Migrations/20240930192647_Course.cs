using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LmsApiApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Course : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseGroup_Group_GroupsId",
                table: "CourseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupEnrollment_AspNetUsers_UserId",
                table: "GroupEnrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupEnrollment_Group_GroupId",
                table: "GroupEnrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupEnrollment",
                table: "GroupEnrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Group",
                table: "Group");

            migrationBuilder.RenameTable(
                name: "GroupEnrollment",
                newName: "GroupEnrollments");

            migrationBuilder.RenameTable(
                name: "Group",
                newName: "Groups");

            migrationBuilder.RenameIndex(
                name: "IX_GroupEnrollment_UserId",
                table: "GroupEnrollments",
                newName: "IX_GroupEnrollments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupEnrollment_GroupId",
                table: "GroupEnrollments",
                newName: "IX_GroupEnrollments_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupEnrollments",
                table: "GroupEnrollments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseGroup_Groups_GroupsId",
                table: "CourseGroup",
                column: "GroupsId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupEnrollments_AspNetUsers_UserId",
                table: "GroupEnrollments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupEnrollments_Groups_GroupId",
                table: "GroupEnrollments",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseGroup_Groups_GroupsId",
                table: "CourseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupEnrollments_AspNetUsers_UserId",
                table: "GroupEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupEnrollments_Groups_GroupId",
                table: "GroupEnrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupEnrollments",
                table: "GroupEnrollments");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "Group");

            migrationBuilder.RenameTable(
                name: "GroupEnrollments",
                newName: "GroupEnrollment");

            migrationBuilder.RenameIndex(
                name: "IX_GroupEnrollments_UserId",
                table: "GroupEnrollment",
                newName: "IX_GroupEnrollment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupEnrollments_GroupId",
                table: "GroupEnrollment",
                newName: "IX_GroupEnrollment_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Group",
                table: "Group",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupEnrollment",
                table: "GroupEnrollment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseGroup_Group_GroupsId",
                table: "CourseGroup",
                column: "GroupsId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupEnrollment_AspNetUsers_UserId",
                table: "GroupEnrollment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupEnrollment_Group_GroupId",
                table: "GroupEnrollment",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
