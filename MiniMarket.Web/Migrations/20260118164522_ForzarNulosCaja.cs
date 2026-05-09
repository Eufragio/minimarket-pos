using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class ForzarNulosCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
{
    // COMANDO SQL DIRECTO PARA OBLIGAR A LA TABLA A ACEPTAR NULOS
    migrationBuilder.Sql("ALTER TABLE AperturasCaja ALTER COLUMN MontoCierre decimal(18,2) NULL");
}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
