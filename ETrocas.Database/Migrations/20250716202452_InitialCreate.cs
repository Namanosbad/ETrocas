using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETrocas.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "produtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Produto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disponivel = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_produtos_usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "proposta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataProposta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataResposta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusProposta = table.Column<int>(type: "int", nullable: false),
                    UsuarioPropostaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoDesejadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoOfertadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proposta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_proposta_produtos_ProdutoDesejadoId",
                        column: x => x.ProdutoDesejadoId,
                        principalTable: "produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_proposta_produtos_ProdutoOfertadoId",
                        column: x => x.ProdutoOfertadoId,
                        principalTable: "produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_proposta_usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_proposta_usuario_UsuarioPropostaId",
                        column: x => x.UsuarioPropostaId,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_produtos_UsuarioId",
                table: "produtos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_proposta_ProdutoDesejadoId",
                table: "proposta",
                column: "ProdutoDesejadoId");

            migrationBuilder.CreateIndex(
                name: "IX_proposta_ProdutoOfertadoId",
                table: "proposta",
                column: "ProdutoOfertadoId");

            migrationBuilder.CreateIndex(
                name: "IX_proposta_UsuarioId",
                table: "proposta",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_proposta_UsuarioPropostaId",
                table: "proposta",
                column: "UsuarioPropostaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "proposta");

            migrationBuilder.DropTable(
                name: "produtos");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
