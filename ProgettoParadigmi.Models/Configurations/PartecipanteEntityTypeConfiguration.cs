using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Models.Configurations
{
    internal class PartecipanteEntityTypeConfiguration : IEntityTypeConfiguration<Partecipante>
    {
        public void Configure(EntityTypeBuilder<Partecipante> builder)
        {
            builder
                .HasOne(x => x.Utente)
                .WithMany(x => x.Partecipazioni)
                .HasForeignKey(x => x.UtenteId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Evento)
                .WithMany(x => x.Partecipanti)
                .HasForeignKey(x => x.EventoId);
            builder
                .Property(x => x.StatoInvito)
                .HasConversion<int>();
            //builder.ToTable("Partecipanti");
        }
    }
}
