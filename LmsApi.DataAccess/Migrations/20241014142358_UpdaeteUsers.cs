using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LmsApiApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdaeteUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordOtp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordOtp",
                table: "AspNetUsers");
        }
    }
}
