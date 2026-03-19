using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneStopMobileRepair.Migrations
{
    /// <inheritdoc />
    public partial class AddEnquiryImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Enquiries",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Enquiries");
        }
    }
}
