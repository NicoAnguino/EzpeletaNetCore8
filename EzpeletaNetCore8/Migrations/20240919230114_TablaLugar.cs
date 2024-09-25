using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzpeletaNetCore8.Migrations
{
    /// <inheritdoc />
    public partial class TablaLugar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LugarID",
                table: "EjerciciosFisicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lugares",
                columns: table => new
                {
                    LugarID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Eliminado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lugares", x => x.LugarID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lugares");

            migrationBuilder.DropColumn(
                name: "LugarID",
                table: "EjerciciosFisicos");
        }
    }
}
