using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Films.Infrastructure.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddedServers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmRooms_Servers_ServerModelId",
                table: "FilmRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeRooms_Servers_ServerModelId",
                table: "YoutubeRooms");

            migrationBuilder.DropIndex(
                name: "IX_YoutubeRooms_ServerModelId",
                table: "YoutubeRooms");

            migrationBuilder.DropIndex(
                name: "IX_FilmRooms_ServerModelId",
                table: "FilmRooms");

            migrationBuilder.DropColumn(
                name: "ServerModelId",
                table: "YoutubeRooms");

            migrationBuilder.DropColumn(
                name: "ServerModelId",
                table: "FilmRooms");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeRooms_ServerId",
                table: "YoutubeRooms",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmRooms_ServerId",
                table: "FilmRooms",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmRooms_Servers_ServerId",
                table: "FilmRooms",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeRooms_Servers_ServerId",
                table: "YoutubeRooms",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmRooms_Servers_ServerId",
                table: "FilmRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeRooms_Servers_ServerId",
                table: "YoutubeRooms");

            migrationBuilder.DropIndex(
                name: "IX_YoutubeRooms_ServerId",
                table: "YoutubeRooms");

            migrationBuilder.DropIndex(
                name: "IX_FilmRooms_ServerId",
                table: "FilmRooms");

            migrationBuilder.AddColumn<Guid>(
                name: "ServerModelId",
                table: "YoutubeRooms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ServerModelId",
                table: "FilmRooms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeRooms_ServerModelId",
                table: "YoutubeRooms",
                column: "ServerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmRooms_ServerModelId",
                table: "FilmRooms",
                column: "ServerModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmRooms_Servers_ServerModelId",
                table: "FilmRooms",
                column: "ServerModelId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeRooms_Servers_ServerModelId",
                table: "YoutubeRooms",
                column: "ServerModelId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
