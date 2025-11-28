using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PamojaWebsite.Migrations
{
    /// <inheritdoc />
    public partial class CareerRoleUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CareerRole",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "CareerRole",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CareerRole");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "CareerRole");
        }
    }
}
