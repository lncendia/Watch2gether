using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                name: "Films",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PosterUrl = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: false),
                    Beep = table.Column<bool>(type: "boolean", nullable: false),
                    Scream = table.Column<bool>(type: "boolean", nullable: false),
                    Change = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Access = table.Column<bool>(type: "boolean", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilmCdns",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    FilmId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmCdns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilmCdns_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FilmId = table.Column<Guid>(type: "uuid", nullable: false),
                    CdnName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    LastActivity = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilmRooms_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeRoomsBannedUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeRoomsBannedUsers", x => new { x.UserId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_YoutubeRoomsBannedUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YoutubeRoomsBannedUsers_YoutubeRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "YoutubeRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeRoomsVideoIds",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    VideoId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeRoomsVideoIds", x => new { x.RoomId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_YoutubeRoomsVideoIds_YoutubeRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "YoutubeRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeViewers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    VideoId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Online = table.Column<bool>(type: "boolean", nullable: false),
                    FullScreen = table.Column<bool>(type: "boolean", nullable: false),
                    Pause = table.Column<bool>(type: "boolean", nullable: false),
                    Owner = table.Column<bool>(type: "boolean", nullable: false),
                    TimeLine = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CustomName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeViewers", x => new { x.UserId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_YoutubeViewers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YoutubeViewers_YoutubeRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "YoutubeRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeViewersMessages",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeViewersMessages", x => new { x.UserId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_YoutubeViewersMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YoutubeViewersMessages_YoutubeRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "YoutubeRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmRoomsBannedUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmRoomsBannedUsers", x => new { x.UserId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_FilmRoomsBannedUsers_FilmRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "FilmRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmRoomsBannedUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmViewers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    Season = table.Column<int>(type: "integer", nullable: true),
                    Series = table.Column<int>(type: "integer", nullable: true),
                    Online = table.Column<bool>(type: "boolean", nullable: false),
                    FullScreen = table.Column<bool>(type: "boolean", nullable: false),
                    Pause = table.Column<bool>(type: "boolean", nullable: false),
                    Owner = table.Column<bool>(type: "boolean", nullable: false),
                    TimeLine = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CustomName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmViewers", x => new { x.UserId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_FilmViewers_FilmRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "FilmRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmViewers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmViewersMessages",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmViewersMessages", x => new { x.UserId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_FilmViewersMessages_FilmRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "FilmRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmViewersMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmCdns_FilmId",
                table: "FilmCdns",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmRooms_FilmId",
                table: "FilmRooms",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmRoomsBannedUsers_RoomId",
                table: "FilmRoomsBannedUsers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmViewers_RoomId",
                table: "FilmViewers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmViewersMessages_RoomId",
                table: "FilmViewersMessages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeRoomsBannedUsers_RoomId",
                table: "YoutubeRoomsBannedUsers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeViewers_RoomId",
                table: "YoutubeViewers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeViewersMessages_RoomId",
                table: "YoutubeViewersMessages",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmCdns");

            migrationBuilder.DropTable(
                name: "FilmRoomsBannedUsers");

            migrationBuilder.DropTable(
                name: "FilmViewers");

            migrationBuilder.DropTable(
                name: "FilmViewersMessages");

            migrationBuilder.DropTable(
                name: "YoutubeRoomsBannedUsers");

            migrationBuilder.DropTable(
                name: "YoutubeRoomsVideoIds");

            migrationBuilder.DropTable(
                name: "YoutubeViewers");

            migrationBuilder.DropTable(
                name: "YoutubeViewersMessages");

            migrationBuilder.DropTable(
                name: "FilmRooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "YoutubeRooms");

            migrationBuilder.DropTable(
                name: "Films");
        }
    }
}
