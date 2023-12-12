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
    internal class UtenteEntityTypeConfiguration : IEntityTypeConfiguration<Utente>
    {
        public void Configure(EntityTypeBuilder<Utente> builder)
        {
            builder
                .Property(x => x.Email)
                .IsRequired();
            builder
                .Property(x => x.Nome)
                .IsRequired();
            builder
                .Property(x => x.Cognome)
                .IsRequired();
            builder
                .Property(x => x.TipoUtente)
                .HasConversion<int>();
            //builder.ToTable("Utenti");
        }
    }
}
