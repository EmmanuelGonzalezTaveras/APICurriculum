using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entidades.Migrations
{
    /// <inheritdoc />
    public partial class TablasPrestamosCarteras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Archivos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoDeEntidad = table.Column<int>(type: "int", nullable: false),
                    EntidadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archivos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carteras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carteras", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "Logs",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Properties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Logs", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "PrestamosCarteras",
                columns: table => new
                {
                    PrestamoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarteraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestamosCarteras", x => new { x.PrestamoId, x.CarteraId });
                    table.ForeignKey(
                        name: "FK_PrestamosCarteras_Carteras_CarteraId",
                        column: x => x.CarteraId,
                        principalTable: "Carteras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrestamosCarteras_Prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "Prestamos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrestamosCarteras_CarteraId",
                table: "PrestamosCarteras",
                column: "CarteraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Archivos");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "PrestamosCarteras");

            migrationBuilder.DropTable(
                name: "Carteras");
        }
    }
}
