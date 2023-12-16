namespace ProgettoParadigmi.Models.Entities;

public class Categoria : BaseEntity
{
    public string Descrizione { get; set; }
    public string Color { get; set; }
    public Guid UserId { get; set; }
    public virtual Utente User { get; set; }
    
    public List<Appuntamento> Appuntamenti { get; set; }
}