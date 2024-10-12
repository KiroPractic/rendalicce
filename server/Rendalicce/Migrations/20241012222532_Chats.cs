
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rendalicce.Migrations
{
    /// <inheritdoc />
    public partial class Chats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LastActivity = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalContent = table.Column<string>(type: "text", nullable: false),
                    Updated = table.Column<bool>(type: "boolean", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessage_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessage_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatUser",
                columns: table => new
                {
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUser", x => new { x.ChatId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_ChatUser_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUser_Users_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessageUser",
                columns: table => new
                {
                    ChatMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeenByParticipantsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessageUser", x => new { x.ChatMessageId, x.SeenByParticipantsId });
                    table.ForeignKey(
                        name: "FK_ChatMessageUser_ChatMessage_ChatMessageId",
                        column: x => x.ChatMessageId,
                        principalTable: "ChatMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessageUser_Users_SeenByParticipantsId",
                        column: x => x.SeenByParticipantsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ChatId",
                table: "ChatMessage",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_SenderId",
                table: "ChatMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessageUser_SeenByParticipantsId",
                table: "ChatMessageUser",
                column: "SeenByParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_ParticipantsId",
                table: "ChatUser",
                column: "ParticipantsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessageUser");

            migrationBuilder.DropTable(
                name: "ChatUser");

            migrationBuilder.DropTable(
                name: "ChatMessage");

            migrationBuilder.DropTable(
                name: "Chats");
        }
    }
}
