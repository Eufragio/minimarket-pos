using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class ControlCajaCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AperturasCaja_AspNetUsers_UsuarioId",
                table: "AperturasCaja");

            migrationBuilder.DropIndex(
                name: "IX_AperturasCaja_UsuarioId",
                table: "AperturasCaja");

            migrationBuilder.DropColumn(
                name: "MontoReal",
                table: "AperturasCaja");

            migrationBuilder.RenameColumn(
                name: "MontoSistema",
                table: "AperturasCaja",
                newName: "MontoCierre");

            migrationBuilder.RenameColumn(
                name: "EstaAbierta",
                table: "AperturasCaja",
                newName: "Estado");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "AperturasCaja",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalVentas",
                table: "AperturasCaja",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalVentas",
                table: "AperturasCaja");

            migrationBuilder.RenameColumn(
                name: "MontoCierre",
                table: "AperturasCaja",
                newName: "MontoSistema");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "AperturasCaja",
                newName: "EstaAbierta");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "AperturasCaja",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoReal",
                table: "AperturasCaja",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AperturasCaja_UsuarioId",
                table: "AperturasCaja",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_AperturasCaja_AspNetUsers_UsuarioId",
                table: "AperturasCaja",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
