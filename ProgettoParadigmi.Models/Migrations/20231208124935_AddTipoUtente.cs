using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgettoParadigmi.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoUtente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoUtente",
                table: "Utenti",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoUtente",
                table: "Utenti");
        }
    }
}
