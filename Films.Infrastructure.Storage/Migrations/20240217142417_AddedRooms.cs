using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Films.Infrastructure.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddedRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servers_Users_OwnerId",
                table: "Servers");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Servers_OwnerId",
                table: "Servers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistFilms",
                table: "PlaylistFilms");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistFilms_FilmId",
                table: "PlaylistFilms");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlaylistFilms");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistFilms",
                table: "PlaylistFilms",
                columns: new[] { "FilmId", "PlaylistId" });

            migrationBuilder.CreateTable(
                name: "FilmRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FilmId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerModelId = table.Column<Guid>(type: "uuid", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_FilmRooms_Servers_ServerModelId",
                        column: x => x.ServerModelId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VideoAccess = table.Column<bool>(type: "boolean", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerModelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeRooms_Servers_ServerModelId",
                        column: x => x.ServerModelId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmViewers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "YoutubeViewers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_FilmRooms_FilmId",
                table: "FilmRooms",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmRooms_ServerModelId",
                table: "FilmRooms",
                column: "ServerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmViewers_RoomId",
                table: "FilmViewers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeRooms_ServerModelId",
                table: "YoutubeRooms",
                column: "ServerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeViewers_RoomId",
                table: "YoutubeViewers",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmViewers");

            migrationBuilder.DropTable(
                name: "YoutubeViewers");

            migrationBuilder.DropTable(
                name: "FilmRooms");

            migrationBuilder.DropTable(
                name: "YoutubeRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistFilms",
                table: "PlaylistFilms");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(40)",
                oldMaxLength: 40);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Servers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "PlaylistFilms",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistFilms",
                table: "PlaylistFilms",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerModelId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsOpen = table.Column<bool>(type: "boolean", nullable: false),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ViewersCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Servers_ServerModelId",
                        column: x => x.ServerModelId,
                        principalTable: "Servers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rooms_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servers_OwnerId",
                table: "Servers",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistFilms_FilmId",
                table: "PlaylistFilms",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_OwnerId",
                table: "Rooms",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ServerModelId",
                table: "Rooms",
                column: "ServerModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servers_Users_OwnerId",
                table: "Servers",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
