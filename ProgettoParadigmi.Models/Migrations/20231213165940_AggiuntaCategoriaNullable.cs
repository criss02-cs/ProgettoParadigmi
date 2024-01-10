using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgettoParadigmi.Models.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaCategoriaNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appuntamenti_Categorie_CategoriaId",
                table: "Appuntamenti");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoriaId",
                table: "Appuntamenti",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Appuntamenti_Categorie_CategoriaId",
                table: "Appuntamenti",
                column: "CategoriaId",
                principalTable: "Categorie",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appuntamenti_Categorie_CategoriaId",
                table: "Appuntamenti");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoriaId",
                table: "Appuntamenti",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appuntamenti_Categorie_CategoriaId",
                table: "Appuntamenti",
                column: "CategoriaId",
                principalTable: "Categorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
