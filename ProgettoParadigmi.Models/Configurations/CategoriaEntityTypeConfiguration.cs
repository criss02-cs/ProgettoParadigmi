using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Models.Configurations;

public class CategoriaEntityTypeConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Categorie)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}