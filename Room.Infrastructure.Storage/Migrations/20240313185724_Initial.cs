using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.Infrastructure.Storage.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilmRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CdnName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CdnUrl = table.Column<string>(type: "text", nullable: false),
                    IsSerial = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VideoAccess = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilmMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilmMessages_FilmRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "FilmRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmViewers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    Season = table.Column<int>(type: "integer", nullable: true),
                    Series = table.Column<int>(type: "integer", nullable: true),
                    Online = table.Column<bool>(type: "boolean", nullable: false),
                    FullScreen = table.Column<bool>(type: "boolean", nullable: false),
                    Pause = table.Column<bool>(type: "boolean", nullable: false),
                    Owner = table.Column<bool>(type: "boolean", nullable: false),
                    TimeLine = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Username = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    Beep = table.Column<bool>(type: "boolean", nullable: false),
                    Scream = table.Column<bool>(type: "boolean", nullable: false),
                    Change = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmViewers", x => new { x.Id, x.RoomId });
                    table.ForeignKey(
                        name: "FK_FilmViewers_FilmRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "FilmRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeMessages_YoutubeRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "YoutubeRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeVideoIds",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    VideoId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeVideoIds", x => new { x.RoomId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_YoutubeVideoIds_YoutubeRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "YoutubeRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeViewers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    VideoId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Online = table.Column<bool>(type: "boolean", nullable: false),
                    FullScreen = table.Column<bool>(type: "boolean", nullable: false),
                    Pause = table.Column<bool>(type: "boolean", nullable: false),
                    Owner = table.Column<bool>(type: "boolean", nullable: false),
                    TimeLine = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Username = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    Beep = table.Column<bool>(type: "boolean", nullable: false),
                    Scream = table.Column<bool>(type: "boolean", nullable: false),
                    Change = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeViewers", x => new { x.Id, x.RoomId });
                    table.ForeignKey(
                        name: "FK_YoutubeViewers_YoutubeRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "YoutubeRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmMessages_RoomId",
                table: "FilmMessages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmViewers_RoomId",
                table: "FilmViewers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeMessages_RoomId",
                table: "YoutubeMessages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeViewers_RoomId",
                table: "YoutubeViewers",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmMessages");

            migrationBuilder.DropTable(
                name: "FilmViewers");

            migrationBuilder.DropTable(
                name: "YoutubeMessages");

            migrationBuilder.DropTable(
                name: "YoutubeVideoIds");

            migrationBuilder.DropTable(
                name: "YoutubeViewers");

            migrationBuilder.DropTable(
                name: "FilmRooms");

            migrationBuilder.DropTable(
                name: "YoutubeRooms");
        }
    }
}
