using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzpeletaNetCore8.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaEjercicioFisico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "TipoEjercicios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EjerciciosFisicos",
                columns: table => new
                {
                    EjercicioFisicoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoEjercicioID = table.Column<int>(type: "int", nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoEmocionalInicio = table.Column<int>(type: "int", nullable: false),
                    EstadoEmocionalFin = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EjerciciosFisicos", x => x.EjercicioFisicoID);
                    table.ForeignKey(
                        name: "FK_EjerciciosFisicos_TipoEjercicios_TipoEjercicioID",
                        column: x => x.TipoEjercicioID,
                        principalTable: "TipoEjercicios",
                        principalColumn: "TipoEjercicioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosFisicos_TipoEjercicioID",
                table: "EjerciciosFisicos",
                column: "TipoEjercicioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EjerciciosFisicos");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "TipoEjercicios");
        }
    }
}
