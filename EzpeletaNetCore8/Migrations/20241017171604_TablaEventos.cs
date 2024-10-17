using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzpeletaNetCore8.Migrations
{
    /// <inheritdoc />
    public partial class TablaEventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventoID",
                table: "EjerciciosFisicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    EventoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Eliminado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.EventoID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosFisicos_LugarID",
                table: "EjerciciosFisicos",
                column: "LugarID");

            migrationBuilder.AddForeignKey(
                name: "FK_EjerciciosFisicos_Lugares_LugarID",
                table: "EjerciciosFisicos",
                column: "LugarID",
                principalTable: "Lugares",
                principalColumn: "LugarID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjerciciosFisicos_Lugares_LugarID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_EjerciciosFisicos_LugarID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropColumn(
                name: "EventoID",
                table: "EjerciciosFisicos");
        }
    }
}
