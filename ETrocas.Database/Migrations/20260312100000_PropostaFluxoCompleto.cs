using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETrocas.Database.Migrations
{
    /// <inheritdoc />
    public partial class PropostaFluxoCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioRecebedorId",
                table: "proposta",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.Sql(@"
                UPDATE p
                SET p.UsuarioRecebedorId = prod.UsuarioId
                FROM proposta p
                INNER JOIN produtos prod ON p.ProdutoDesejadoId = prod.Id
            ");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataResposta",
                table: "proposta",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.DropForeignKey(
                name: "FK_proposta_usuario_UsuarioId",
                table: "proposta");

            migrationBuilder.DropIndex(
                name: "IX_proposta_UsuarioId",
                table: "proposta");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "proposta");

            migrationBuilder.CreateIndex(
                name: "IX_proposta_UsuarioRecebedorId",
                table: "proposta",
                column: "UsuarioRecebedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_proposta_usuario_UsuarioRecebedorId",
                table: "proposta",
                column: "UsuarioRecebedorId",
                principalTable: "usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_proposta_usuario_UsuarioRecebedorId",
                table: "proposta");

            migrationBuilder.DropIndex(
                name: "IX_proposta_UsuarioRecebedorId",
                table: "proposta");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "proposta",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE p
                SET p.UsuarioId = p.UsuarioRecebedorId
                FROM proposta p
            ");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataResposta",
                table: "proposta",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "UsuarioRecebedorId",
                table: "proposta");

            migrationBuilder.CreateIndex(
                name: "IX_proposta_UsuarioId",
                table: "proposta",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_proposta_usuario_UsuarioId",
                table: "proposta",
                column: "UsuarioId",
                principalTable: "usuario",
                principalColumn: "Id");
        }
    }
}
