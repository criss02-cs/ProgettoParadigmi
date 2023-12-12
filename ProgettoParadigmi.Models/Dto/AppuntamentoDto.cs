using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Models.Dto;

public record AppuntamentoDto
{
    public string Titolo { get; set; }
    public string Descrizione { get; set; }
    public DateTime DataInizio { get; set; }
    public DateTime? DataFine { get; set; }
    public Guid OrganizzatoreId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Guid> Partecipanti { get; set; }
}

public record AppuntamentoDaAccettareDto
{
    public string Titolo { get; set; }
    public string Descrizione { get; set; }
    public DateTime DataInizio { get; set; }
    public DateTime? DataFine { get; set; }
    public UtenteDto Organizzatore { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<UtenteDto> Partecipanti { get; set; }
}

public record AggiornaStatoInvitoDto
{
    [Required]
    public Guid PartecipazioneId { get; set; } 
    [Required]
    public StatoInvito Stato { get; set; }
}