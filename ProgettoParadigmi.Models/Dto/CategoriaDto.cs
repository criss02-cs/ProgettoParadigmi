using System.Drawing;

namespace ProgettoParadigmi.Models.Dto;

public record CategoriaDto(string Descrizione, Guid Id, string Color)
{
    public Guid UserId { get; set; }
}