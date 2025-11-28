using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PamojaWebsite.Migrations
{
    /// <inheritdoc />
    public partial class CareerFieldsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "CareerField",
                newName: "ImageFile");

            migrationBuilder.AddColumn<string>(
                name: "ImagePreviewUrl",
                table: "CareerField",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePreviewUrl",
                table: "CareerField");

            migrationBuilder.RenameColumn(
                name: "ImageFile",
                table: "CareerField",
                newName: "Image");
        }
    }
}
