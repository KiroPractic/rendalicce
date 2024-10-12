using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rendalicce.Migrations
{
    /// <inheritdoc />
    public partial class ServiceProvidersExpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "ServiceProviders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ServiceProviders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Geolocation",
                table: "ServiceProviders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ServiceProviders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "Geolocation",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ServiceProviders");
        }
    }
}
