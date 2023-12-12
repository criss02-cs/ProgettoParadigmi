using ProgettoParadigmi.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Models.Dto
{
    public record RegisterDto
    {
        [Required(ErrorMessage = "Il nome è obbligatorio")]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "Il cognome è obbligatorio")]
        public string? Cognome { get; set; }
        [Required(ErrorMessage = "Il l'email è obbligatoria"), EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "La password è obbligatoria")]
        [Password(8, ErrorMessage = "La password deve essere lunga almeno 8 caratteri")]
        public string? Password { get; set; }
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Il l'email è obbligatoria"), EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "La password è obbligatoria")]
        [Password(8, ErrorMessage = "La password deve essere lunga almeno 8 caratteri")]
        public string? Password { get; set; }
    }

    public record AuthDto(string Nome, string Cognome, string Email, Guid Id, TipoUtente TipoUtente)
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Token { get; set; }
    }
}
