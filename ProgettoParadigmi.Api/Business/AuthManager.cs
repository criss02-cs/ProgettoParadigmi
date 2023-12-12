using ProgettoParadigmi.Api.Utils;
using ProgettoParadigmi.Models.Context;
using ProgettoParadigmi.Models.Dto;
using ProgettoParadigmi.Models.Entities;
using ProgettoParadigmi.Models.GenericRepository;

namespace ProgettoParadigmi.Api.Business
{
    public class AuthManager(AppuntamentiDbContext ctx)
    {
        private readonly IGenericRepository<Utente> _repository = new GenericRepository<Utente>(ctx);

        public Response<AuthDto> RegisterUser(RegisterDto dto)
        {
            try
            {
                var user = _repository.GetFirstOrDefault(x => !x.IsDeleted && x.Email == dto.Email);
                if (user is not null)
                    return ResponseFactory.CreateResponseFromResult<AuthDto>(null, false, "Email già esistente");
                var hashedPassword = Encrypter.ComputeHash(dto.Password!);
                var utente = new Utente
                {
                    Cognome = dto.Cognome!,
                    Nome = dto.Nome!,
                    Email = dto.Email!,
                    PasswordHash = hashedPassword,
                    TipoUtente = TipoUtente.Utente
                };
                _repository.Insert(utente);
                var succesful = _repository.SaveChanges();
                if (!succesful)
                {
                    return ResponseFactory
                        .CreateResponseFromResult<AuthDto>(null, false, "C'è stato un errore nella creazione dell'utente, riprova");
                }
                var authDto = new AuthDto(utente.Nome, utente.Cognome, utente.Email, utente.Id, utente.TipoUtente);
                var response = ResponseFactory.CreateResponseFromResult(authDto);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return ResponseFactory
                    .CreateResponseFromResult<AuthDto>(null, false, e.Message);
            }
        }

        public Response<AuthDto> LoginUser(LoginDto dto)
        {
            var user = _repository.GetFirstOrDefault(x => x.Email == dto.Email && !x.IsDeleted);
            if (user is null)
            {
                return ResponseFactory
                    .CreateResponseFromResult<AuthDto>(null, false, "Non esiste un utente con quella email.");
            }

            if (user.PasswordHash != Encrypter.ComputeHash(dto.Password))
            {
                return ResponseFactory
                    .CreateResponseFromResult<AuthDto>(null, false, "La password è errata");
            }
            // a questo punto le credenziali sono quelle giuste
            var authDto = new AuthDto(user.Nome, user.Cognome, user.Email, user.Id, user.TipoUtente);
            return ResponseFactory.CreateResponseFromResult(authDto);
        }
    }
}
