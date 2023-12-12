using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Models.Attributes;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Models.Dto
{
    public record UtenteDto(Guid Id, string Nome, string Cognome, string Email, TipoUtente TipoUtente);

    public record ChangePasswordDto
    {
        [Required(ErrorMessage = "L'id è obbligatorio")]
        public Guid Id { get; set; }
        [Required, Password(8, ErrorMessage = "La password deve essere lunga almeno 8 caratteri")]
        public string Password { get; set; }
    }
}
