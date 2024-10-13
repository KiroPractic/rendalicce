using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rendalicce.Migrations
{
    /// <inheritdoc />
    public partial class UserCreditsBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditsBalance",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditsBalance",
                table: "Users");
        }
    }
}
