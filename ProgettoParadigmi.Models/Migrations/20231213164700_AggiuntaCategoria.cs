using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgettoParadigmi.Models.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoriaId",
                table: "Appuntamenti",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Categorie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorie_Utenti_UserId",
                        column: x => x.UserId,
                        principalTable: "Utenti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appuntamenti_CategoriaId",
                table: "Appuntamenti",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorie_UserId",
                table: "Categorie",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appuntamenti_Categorie_CategoriaId",
                table: "Appuntamenti",
                column: "CategoriaId",
                principalTable: "Categorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appuntamenti_Categorie_CategoriaId",
                table: "Appuntamenti");

            migrationBuilder.DropTable(
                name: "Categorie");

            migrationBuilder.DropIndex(
                name: "IX_Appuntamenti_CategoriaId",
                table: "Appuntamenti");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Appuntamenti");
        }
    }
}
