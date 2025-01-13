using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementService.Migrations
{
    /// <inheritdoc />
    public partial class LibraryBookCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "LibraryBook",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "LibraryBook");
        }
    }
}
