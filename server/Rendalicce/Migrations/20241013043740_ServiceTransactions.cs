using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rendalicce.Migrations
{
    /// <inheritdoc />
    public partial class ServiceTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ChatId",
                table: "ChatMessages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceTransactionId",
                table: "ChatMessages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServiceTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTransactionParticipant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Approved = table.Column<bool>(type: "boolean", nullable: false),
                    Credits = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceTransactionId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProvidedService = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTransactionParticipant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceTransactionParticipant_ServiceTransactions_ServiceTr~",
                        column: x => x.ServiceTransactionId,
                        principalTable: "ServiceTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceTransactionParticipant_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ServiceTransactionId",
                table: "ChatMessages",
                column: "ServiceTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTransactionParticipant_ServiceTransactionId",
                table: "ServiceTransactionParticipant",
                column: "ServiceTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTransactionParticipant_UserId",
                table: "ServiceTransactionParticipant",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ServiceTransactions_ServiceTransactionId",
                table: "ChatMessages",
                column: "ServiceTransactionId",
                principalTable: "ServiceTransactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ServiceTransactions_ServiceTransactionId",
                table: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ServiceTransactionParticipant");

            migrationBuilder.DropTable(
                name: "ServiceTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ServiceTransactionId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ServiceTransactionId",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChatId",
                table: "ChatMessages",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
