using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sireen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRememberMe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRememberMe",
                table: "RefreshToken",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRememberMe",
                table: "RefreshToken");
        }
    }
}
