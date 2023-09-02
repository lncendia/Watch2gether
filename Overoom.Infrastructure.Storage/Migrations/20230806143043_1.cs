using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overoom.Infrastructure.Storage.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddAccess",
                table: "YoutubeRooms",
                newName: "Access");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Access",
                table: "YoutubeRooms",
                newName: "AddAccess");
        }
    }
}
