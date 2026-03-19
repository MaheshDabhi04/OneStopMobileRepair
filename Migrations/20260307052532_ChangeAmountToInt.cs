using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneStopMobileRepair.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAmountToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Estimate",
                table: "Customers",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,0)");

            migrationBuilder.AlterColumn<int>(
                name: "Deposit",
                table: "Customers",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Estimate",
                table: "Customers",
                type: "decimal(10,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Deposit",
                table: "Customers",
                type: "decimal(10,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
