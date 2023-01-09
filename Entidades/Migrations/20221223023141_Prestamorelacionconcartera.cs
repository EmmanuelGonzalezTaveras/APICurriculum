using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entidades.Migrations
{
    /// <inheritdoc />
    public partial class Prestamorelacionconcartera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "20528816-cbe7-47cb-a6f2-1d9e3d4ad718",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd67968a-7c8b-4bfe-bef5-0ed498cdd6a0", "AQAAAAIAAYagAAAAEM0Y13vxUClII79F6C4mtFH3tmYK0EUVbBWmHdGk111QDfigiz0XoHwjdCjkfE6DTA==", "6d0b1d33-0fef-47d5-9b1b-ae78cd6d462e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "20528816-cbe7-47cb-a6f2-1d9e3d4ad718",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "20d5a528-16fd-4ec8-adf7-7eed781eeffb", "AQAAAAIAAYagAAAAEJ5gLi4XRG1iaLQQWEgPPT/yv31I1sizou87EqPt3p3QpgDrStAQbYmKZ24cH+0Muw==", "04f70581-b0ca-4b5a-b69c-4e08a9bb67d3" });
        }
    }
}
