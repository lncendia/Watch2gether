using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overoom.Infrastructure.PersistentStorage.Migrations
{
    public partial class mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Viewers_ViewerId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Viewers");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "RoomBaseModel");

            migrationBuilder.CreateTable(
                name: "VideoIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VideoId = table.Column<string>(type: "TEXT", nullable: false),
                    RoomId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoIds_RoomBaseModel_RoomId",
                        column: x => x.RoomId,
                        principalTable: "RoomBaseModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViewerBaseModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RoomId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AvatarFileName = table.Column<string>(type: "TEXT", nullable: false),
                    Online = table.Column<bool>(type: "INTEGER", nullable: false),
                    OnPause = table.Column<bool>(type: "INTEGER", nullable: false),
                    TimeLine = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Season = table.Column<int>(type: "INTEGER", nullable: true),
                    Series = table.Column<int>(type: "INTEGER", nullable: true),
                    CurrentVideoId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewerBaseModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViewerBaseModel_RoomBaseModel_RoomId",
                        column: x => x.RoomId,
                        principalTable: "RoomBaseModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoIds_RoomId",
                table: "VideoIds",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ViewerBaseModel_RoomId",
                table: "ViewerBaseModel",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ViewerBaseModel_ViewerId",
                table: "Messages",
                column: "ViewerId",
                principalTable: "ViewerBaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ViewerBaseModel_ViewerId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "VideoIds");

            migrationBuilder.DropTable(
                name: "ViewerBaseModel");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "RoomBaseModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Viewers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoomId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AvatarFileName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    OnPause = table.Column<bool>(type: "INTEGER", nullable: false),
                    Online = table.Column<bool>(type: "INTEGER", nullable: false),
                    TimeLine = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Viewers_RoomBaseModel_RoomId",
                        column: x => x.RoomId,
                        principalTable: "RoomBaseModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Viewers_RoomId",
                table: "Viewers",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Viewers_ViewerId",
                table: "Messages",
                column: "ViewerId",
                principalTable: "Viewers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
