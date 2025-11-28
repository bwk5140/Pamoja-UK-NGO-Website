using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PamojaWebsite.Migrations
{
    /// <inheritdoc />
    public partial class CareerFieldUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CareerField",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "CareerField",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "CareerField");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "CareerField");
        }
    }
}
