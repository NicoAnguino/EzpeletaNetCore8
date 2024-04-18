using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzpeletaNetCore8.Migrations
{
    /// <inheritdoc />
    public partial class CrearTablaTipoEjercicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoEjercicios",
                columns: table => new
                {
                    TipoEjercicioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEjercicios", x => x.TipoEjercicioID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoEjercicios");
        }
    }
}
