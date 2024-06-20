using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Licencjat.Data.Migrations
{
    /// <inheritdoc />
    public partial class obrazki : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Dish",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Dish");
        }
    }
}
