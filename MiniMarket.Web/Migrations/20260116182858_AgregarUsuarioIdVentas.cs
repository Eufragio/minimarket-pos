using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class AgregarUsuarioIdVentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
{
    // PEGA ESTO AQUÍ ADENTRO:
    migrationBuilder.AddColumn<string>(
        name: "UsuarioId",
        table: "Ventas",
        type: "nvarchar(450)",
        nullable: true);

    migrationBuilder.CreateIndex(
        name: "IX_Ventas_UsuarioId",
        table: "Ventas",
        column: "UsuarioId");

    migrationBuilder.AddForeignKey(
        name: "FK_Ventas_AspNetUsers_UsuarioId",
        table: "Ventas",
        column: "UsuarioId",
        principalTable: "AspNetUsers",
        principalColumn: "Id");
}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
