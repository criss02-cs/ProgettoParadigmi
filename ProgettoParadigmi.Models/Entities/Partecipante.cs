using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoParadigmi.Models.Entities
{
    public class Partecipante : BaseEntity
    {
        public Guid UtenteId { get; set; }
        public Utente Utente { get; set; }

        public Guid EventoId { get; set; }
        public Appuntamento Evento { get; set; }

        public StatoInvito StatoInvito { get; set; }
    }

    public enum StatoInvito
    {
        Inviato,
        Accettato,
        Rifiutato
    }
}
