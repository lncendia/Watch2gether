using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Films.Infrastructure.Storage.Migrations
{
    /// <inheritdoc />
    public partial class FilmTypeToBoolChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Films");

            migrationBuilder.AddColumn<bool>(
                name: "IsSerial",
                table: "Films",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSerial",
                table: "Films");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Films",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
