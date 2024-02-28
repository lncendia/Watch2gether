using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Films.Infrastructure.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddedBannedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CdnName",
                table: "FilmRooms",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BannedFilmViewers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedFilmViewers", x => new { x.UserId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_BannedFilmViewers_FilmRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "FilmRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannedFilmViewers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BannedYoutubeViewers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedYoutubeViewers", x => new { x.UserId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_BannedYoutubeViewers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannedYoutubeViewers_YoutubeRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "YoutubeRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannedFilmViewers_RoomId",
                table: "BannedFilmViewers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_BannedYoutubeViewers_RoomId",
                table: "BannedYoutubeViewers",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannedFilmViewers");

            migrationBuilder.DropTable(
                name: "BannedYoutubeViewers");

            migrationBuilder.DropColumn(
                name: "CdnName",
                table: "FilmRooms");
        }
    }
}
