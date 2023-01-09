using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entidades.Migrations
{
    /// <inheritdoc />
    public partial class TablaSeguimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seguimientos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrestamoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Anotacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeguimientoPendiente = table.Column<bool>(type: "bit", nullable: false),
                    FechaDeSeguimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seguimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seguimientos_AspNetUsers_UsuarioId1",
                        column: x => x.UsuarioId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "20528816-cbe7-47cb-a6f2-1d9e3d4ad718",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "20d5a528-16fd-4ec8-adf7-7eed781eeffb", "AQAAAAIAAYagAAAAEJ5gLi4XRG1iaLQQWEgPPT/yv31I1sizou87EqPt3p3QpgDrStAQbYmKZ24cH+0Muw==", "04f70581-b0ca-4b5a-b69c-4e08a9bb67d3" });

            migrationBuilder.CreateIndex(
                name: "IX_Seguimientos_UsuarioId1",
                table: "Seguimientos",
                column: "UsuarioId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seguimientos");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "20528816-cbe7-47cb-a6f2-1d9e3d4ad718",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac412de9-84f0-49e3-9dfc-ea2e3123de8b", "AQAAAAIAAYagAAAAEMFy84+jlUhdEB8e45oUWEoZmDVaVZN60gvdOPWgBJN7qD5AhjJSRpxSGA2Zgx328Q==", "133056ce-528f-4d48-b244-f05ed4512b09" });
        }
    }
}
