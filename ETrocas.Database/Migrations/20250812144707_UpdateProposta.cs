using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETrocas.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProposta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mensagem",
                table: "proposta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorProposto",
                table: "proposta",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mensagem",
                table: "proposta");

            migrationBuilder.DropColumn(
                name: "ValorProposto",
                table: "proposta");
        }
    }
}
