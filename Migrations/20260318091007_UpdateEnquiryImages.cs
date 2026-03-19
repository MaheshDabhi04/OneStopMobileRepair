using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneStopMobileRepair.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEnquiryImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Enquiries",
                newName: "FrontImagePath");

            migrationBuilder.AddColumn<string>(
                name: "BackImagePath",
                table: "Enquiries",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackImagePath",
                table: "Enquiries");

            migrationBuilder.RenameColumn(
                name: "FrontImagePath",
                table: "Enquiries",
                newName: "ImagePath");
        }
    }
}
