using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP.Database.Migrations
{
    /// <inheritdoc />
    public partial class Addednametovenue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Venues",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Venues");
        }
    }
}
