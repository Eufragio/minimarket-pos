using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarModeloCompras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroComprobante",
                table: "Compras");

            migrationBuilder.RenameColumn(
                name: "CostoUnitario",
                table: "DetalleCompras",
                newName: "PrecioUnitario");

            migrationBuilder.AddColumn<string>(
                name: "NumeroDocumento",
                table: "Compras",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Compras",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "AperturasCaja",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compras_UsuarioId",
                table: "Compras",
                column: "UsuarioId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_AspNetUsers_UsuarioId",
                table: "Compras",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AperturasCaja_AspNetUsers_UsuarioId",
                table: "AperturasCaja");

            migrationBuilder.DropForeignKey(
                name: "FK_Compras_AspNetUsers_UsuarioId",
                table: "Compras");

            migrationBuilder.DropIndex(
                name: "IX_Compras_UsuarioId",
                table: "Compras");

            migrationBuilder.DropIndex(
                name: "IX_AperturasCaja_UsuarioId",
                table: "AperturasCaja");

            migrationBuilder.DropColumn(
                name: "NumeroDocumento",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Compras");

            migrationBuilder.RenameColumn(
                name: "PrecioUnitario",
                table: "DetalleCompras",
                newName: "CostoUnitario");

            migrationBuilder.AddColumn<string>(
                name: "NumeroComprobante",
                table: "Compras",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "AperturasCaja",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
