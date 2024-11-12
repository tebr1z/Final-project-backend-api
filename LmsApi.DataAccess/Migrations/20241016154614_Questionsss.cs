using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LmsApiApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Questionsss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Questions");
        }
    }
}
