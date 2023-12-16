using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoParadigmi.Models.Entities
{
    public class Utente : BaseEntity
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public TipoUtente TipoUtente { get; set; }

        // Appuntamenti creati
        public List<Appuntamento> Appuntamenti { get; set; }

        // Appuntamenti a cui si è stato invitato
        public List<Partecipante> Partecipazioni { get; set; }
        
        // Categorie create
        public List<Categoria> Categorie { get; set; }
    }

    public enum TipoUtente
    {
        Admin,
        Utente
    }
}
