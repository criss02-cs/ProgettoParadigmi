using Microsoft.EntityFrameworkCore;
using ProgettoParadigmi.Models.Configurations;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Models.Context;

public class AppuntamentiDbContext(DbContextOptions<AppuntamentiDbContext> opt) : DbContext(opt)
{
    public DbSet<Utente> Utenti { get; set; }
    public DbSet<Partecipante> Partecipanti { get; set; }
    public DbSet<Appuntamento> Appuntamenti { get; set; }
    public DbSet<Categoria> Categorie { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        new AppuntamentoEntityTypeConfiguration().Configure(modelBuilder.Entity<Appuntamento>());
        new PartecipanteEntityTypeConfiguration().Configure(modelBuilder.Entity<Partecipante>());
        new UtenteEntityTypeConfiguration().Configure(modelBuilder.Entity<Utente>());
        new CategoriaEntityTypeConfiguration().Configure(modelBuilder.Entity<Categoria>());
    }
}