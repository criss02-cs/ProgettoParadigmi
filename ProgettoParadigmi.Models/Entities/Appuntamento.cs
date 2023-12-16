using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoParadigmi.Models.Entities
{
    public class Appuntamento : BaseEntity
    {
        public string Titolo { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime? DataFine { get; set; }
        // Foreign Key con l'utente
        public Guid OrganizzatoreId { get; set; }
        public Utente Organizzatore { get; set; }
        
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public List<Partecipante> Partecipanti { get; set; }
    }
}
