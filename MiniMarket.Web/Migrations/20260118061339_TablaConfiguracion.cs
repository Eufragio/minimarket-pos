using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class TablaConfiguracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.RenameColumn(
                name: "RUC",
                table: "Configuraciones",
                newName: "Ruc");

            migrationBuilder.RenameColumn(
                name: "ZonaHorariaId",
                table: "Configuraciones",
                newName: "EmailContacto");

            migrationBuilder.AlterColumn<string>(
                name: "Ruc",
                table: "Configuraciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NombreEmpresa",
                table: "Configuraciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "IgvPorcentaje",
                table: "Configuraciones",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IgvPorcentaje",
                table: "Configuraciones");

            migrationBuilder.RenameColumn(
                name: "Ruc",
                table: "Configuraciones",
                newName: "RUC");

            migrationBuilder.RenameColumn(
                name: "EmailContacto",
                table: "Configuraciones",
                newName: "ZonaHorariaId");

            migrationBuilder.AlterColumn<string>(
                name: "RUC",
                table: "Configuraciones",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NombreEmpresa",
                table: "Configuraciones",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Configuraciones",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
