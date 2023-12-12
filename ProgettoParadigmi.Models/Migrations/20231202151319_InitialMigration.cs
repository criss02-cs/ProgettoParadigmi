using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgettoParadigmi.Models.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appuntamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titolo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInizio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFine = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizzatoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appuntamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appuntamento_Utente_OrganizzatoreId",
                        column: x => x.OrganizzatoreId,
                        principalTable: "Utente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Partecipante",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UtenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partecipante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partecipante_Appuntamento_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Appuntamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Partecipante_Utente_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appuntamento_OrganizzatoreId",
                table: "Appuntamento",
                column: "OrganizzatoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Partecipante_EventoId",
                table: "Partecipante",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_Partecipante_UtenteId",
                table: "Partecipante",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partecipante");

            migrationBuilder.DropTable(
                name: "Appuntamento");

            migrationBuilder.DropTable(
                name: "Utente");
        }
    }
}
