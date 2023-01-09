using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entidades.Migrations
{
    /// <inheritdoc />
    public partial class AdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ab9b9283-721d-4311-9333-f9c8989996a1", null, "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "20528816-cbe7-47cb-a6f2-1d9e3d4ad718", 0, "ac412de9-84f0-49e3-9dfc-ea2e3123de8b", "emmanuel.g.t@hotmail.com", false, false, null, "emmanuel.g.t@hotmail.com", "emmanuel.g.t@hotmail.com", "AQAAAAIAAYagAAAAEMFy84+jlUhdEB8e45oUWEoZmDVaVZN60gvdOPWgBJN7qD5AhjJSRpxSGA2Zgx328Q==", null, false, "133056ce-528f-4d48-b244-f05ed4512b09", false, "emmanuel.g.t@hotmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "20528816-cbe7-47cb-a6f2-1d9e3d4ad718" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab9b9283-721d-4311-9333-f9c8989996a1");

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "20528816-cbe7-47cb-a6f2-1d9e3d4ad718");
        }
    }
}
