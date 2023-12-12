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
    internal class AppuntamentoEntityTypeConfiguration : IEntityTypeConfiguration<Appuntamento>
    {
        public void Configure(EntityTypeBuilder<Appuntamento> builder)
        {
            builder
                .HasOne(x => x.Organizzatore)
                .WithMany(x => x.Appuntamenti)
                .HasForeignKey(x => x.OrganizzatoreId);
            //builder.ToTable("Appuntamenti");
        }
    }
}
