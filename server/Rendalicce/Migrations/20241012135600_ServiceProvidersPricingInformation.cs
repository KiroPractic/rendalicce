using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rendalicce.Migrations
{
    /// <inheritdoc />
    public partial class ServiceProvidersPricingInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "ServiceProviders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ServiceProviders",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceProviders");
        }
    }
}
