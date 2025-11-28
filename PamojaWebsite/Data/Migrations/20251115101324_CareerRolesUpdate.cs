using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PamojaWebsite.Migrations
{
    /// <inheritdoc />
    public partial class CareerRolesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExperienceLevel",
                table: "CareerRole",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "CareerRole",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkType",
                table: "CareerRole",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceLevel",
                table: "CareerRole");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "CareerRole");

            migrationBuilder.DropColumn(
                name: "WorkType",
                table: "CareerRole");
        }
    }
}
