using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneStopMobileRepair.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            var user = new IdentityUser
            {
                Id = "admin-id-001",
                UserName = "jd@onestop.com",
                NormalizedUserName = "JD@ONESTOP.COM",
                Email = "jd@onestop.com",
                NormalizedEmail = "JD@ONESTOP.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            user.PasswordHash = hasher.HashPassword(user, "Jaysukh@3877");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
            "Id","UserName","NormalizedUserName","Email","NormalizedEmail",
            "EmailConfirmed","PasswordHash","SecurityStamp","ConcurrencyStamp",
            "PhoneNumberConfirmed","TwoFactorEnabled","LockoutEnabled","AccessFailedCount"
                },
                values: new object[]
                {
            user.Id,
            user.UserName,
            user.NormalizedUserName,
            user.Email,
            user.NormalizedEmail,
            user.EmailConfirmed,
            user.PasswordHash,
            user.SecurityStamp,
            Guid.NewGuid().ToString(),
            false,
            false,
            true,
            0
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
