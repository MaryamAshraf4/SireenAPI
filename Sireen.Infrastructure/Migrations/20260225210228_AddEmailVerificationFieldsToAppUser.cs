using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sireen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailVerificationFieldsToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailCodeExpiration",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailCodeExpiration",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailVerificationCode",
                table: "AspNetUsers");
        }
    }
}
