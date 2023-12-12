using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgettoParadigmi.Models.Migrations
{
    /// <inheritdoc />
    public partial class changedtablenames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appuntamento_Utente_OrganizzatoreId",
                table: "Appuntamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Partecipante_Appuntamento_EventoId",
                table: "Partecipante");

            migrationBuilder.DropForeignKey(
                name: "FK_Partecipante_Utente_UtenteId",
                table: "Partecipante");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Utente",
                table: "Utente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Partecipante",
                table: "Partecipante");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appuntamento",
                table: "Appuntamento");

            migrationBuilder.RenameTable(
                name: "Utente",
                newName: "Utenti");

            migrationBuilder.RenameTable(
                name: "Partecipante",
                newName: "Partecipanti");

            migrationBuilder.RenameTable(
                name: "Appuntamento",
                newName: "Appuntamenti");

            migrationBuilder.RenameIndex(
                name: "IX_Partecipante_UtenteId",
                table: "Partecipanti",
                newName: "IX_Partecipanti_UtenteId");

            migrationBuilder.RenameIndex(
                name: "IX_Partecipante_EventoId",
                table: "Partecipanti",
                newName: "IX_Partecipanti_EventoId");

            migrationBuilder.RenameIndex(
                name: "IX_Appuntamento_OrganizzatoreId",
                table: "Appuntamenti",
                newName: "IX_Appuntamenti_OrganizzatoreId");

            migrationBuilder.AddColumn<int>(
                name: "StatoInvito",
                table: "Partecipanti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Utenti",
                table: "Utenti",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Partecipanti",
                table: "Partecipanti",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appuntamenti",
                table: "Appuntamenti",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appuntamenti_Utenti_OrganizzatoreId",
                table: "Appuntamenti",
                column: "OrganizzatoreId",
                principalTable: "Utenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partecipanti_Appuntamenti_EventoId",
                table: "Partecipanti",
                column: "EventoId",
                principalTable: "Appuntamenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partecipanti_Utenti_UtenteId",
                table: "Partecipanti",
                column: "UtenteId",
                principalTable: "Utenti",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appuntamenti_Utenti_OrganizzatoreId",
                table: "Appuntamenti");

            migrationBuilder.DropForeignKey(
                name: "FK_Partecipanti_Appuntamenti_EventoId",
                table: "Partecipanti");

            migrationBuilder.DropForeignKey(
                name: "FK_Partecipanti_Utenti_UtenteId",
                table: "Partecipanti");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Utenti",
                table: "Utenti");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Partecipanti",
                table: "Partecipanti");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appuntamenti",
                table: "Appuntamenti");

            migrationBuilder.DropColumn(
                name: "StatoInvito",
                table: "Partecipanti");

            migrationBuilder.RenameTable(
                name: "Utenti",
                newName: "Utente");

            migrationBuilder.RenameTable(
                name: "Partecipanti",
                newName: "Partecipante");

            migrationBuilder.RenameTable(
                name: "Appuntamenti",
                newName: "Appuntamento");

            migrationBuilder.RenameIndex(
                name: "IX_Partecipanti_UtenteId",
                table: "Partecipante",
                newName: "IX_Partecipante_UtenteId");

            migrationBuilder.RenameIndex(
                name: "IX_Partecipanti_EventoId",
                table: "Partecipante",
                newName: "IX_Partecipante_EventoId");

            migrationBuilder.RenameIndex(
                name: "IX_Appuntamenti_OrganizzatoreId",
                table: "Appuntamento",
                newName: "IX_Appuntamento_OrganizzatoreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Utente",
                table: "Utente",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Partecipante",
                table: "Partecipante",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appuntamento",
                table: "Appuntamento",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appuntamento_Utente_OrganizzatoreId",
                table: "Appuntamento",
                column: "OrganizzatoreId",
                principalTable: "Utente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partecipante_Appuntamento_EventoId",
                table: "Partecipante",
                column: "EventoId",
                principalTable: "Appuntamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partecipante_Utente_UtenteId",
                table: "Partecipante",
                column: "UtenteId",
                principalTable: "Utente",
                principalColumn: "Id");
        }
    }
}
