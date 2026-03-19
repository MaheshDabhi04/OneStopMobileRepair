using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneStopMobileRepair.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCustomerImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackImage",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FrontImage",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackImage",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontImage",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
