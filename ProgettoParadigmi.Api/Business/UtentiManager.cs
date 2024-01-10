using ProgettoParadigmi.Api.Utils;
using ProgettoParadigmi.Models.Context;
using ProgettoParadigmi.Models.Dto;
using ProgettoParadigmi.Models.Entities;
using ProgettoParadigmi.Models.GenericRepository;

namespace ProgettoParadigmi.Api.Business
{
    public class UtentiManager(AppuntamentiDbContext ctx)
    {
        private readonly IGenericRepository<Utente> _repository = new GenericRepository<Utente>(ctx);
        public Response<List<UtenteDto>> GetAll(int take = 10, int skip = 0, string filtro = "")
        {
            var users = _repository
                .Query(filter: x => !x.IsDeleted
                    && x.Email.ToLower().Contains(filtro.ToLower()), orderBy: x => x.OrderBy(y => y.Cognome))
                .Skip(skip)
                .Take(take)
                .Select(x => new UtenteDto(x.Id, x.Nome, x.Cognome, x.Email, x.TipoUtente))
                .ToList();
            return ResponseFactory.CreateResponseFromResult(users);
        }

        public Response<bool> UpdateUser(UtenteDto dto)
        {
            if (dto.Id == Guid.Empty)
                return ResponseFactory.CreateResponseFromResult(false, false, "L'id non può essere vuoto");
            var user = _repository.GetFirstOrDefault(x => x.Id == dto.Id);
            if(user is null)
                return ResponseFactory.CreateResponseFromResult(false, false, "Non esiste un utente con questo id");
            user.Nome = dto.Nome;
            user.TipoUtente = dto.TipoUtente;
            user.Cognome = dto.Cognome;
            user.Email = dto.Email;
            _repository.Update(user);
            var result = _repository.SaveChanges();
            return ResponseFactory.CreateResponseFromResult(result);
        }

        public Response<bool> ChangePassword(ChangePasswordDto dto)
        {
            if (dto.Id == Guid.Empty)
                return ResponseFactory.CreateResponseFromResult(false, false, "L'id non può essere vuoto");
            var user = _repository.GetFirstOrDefault(x => x.Id == dto.Id);
            if (user is null)
                return ResponseFactory.CreateResponseFromResult(false, false, "Non esiste un utente con questo id");
            user.PasswordHash = Encrypter.ComputeHash(dto.Password);
            _repository.Update(user);
            var result = _repository.SaveChanges();
            return ResponseFactory.CreateResponseFromResult(result);
        }

        public Response<UtenteDto> GetUtenteById(Guid id)
        {
            if (id == Guid.Empty)
                return ResponseFactory.CreateResponseFromResult<UtenteDto>(null, false, "L'id non può essere vuoto");
            var user = _repository.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (user is null)
                return ResponseFactory.CreateResponseFromResult<UtenteDto>(null, false, "Non esiste un utente con questo id");
            var dto = new UtenteDto(user.Id, user.Nome, user.Cognome, user.Email, user.TipoUtente);
            return ResponseFactory.CreateResponseFromResult(dto);
        }

        public Response<bool> EliminaUtente(Guid id)
        {
            if (id == Guid.Empty)
                return ResponseFactory.CreateResponseFromResult(false, false, "L'id non può essere vuoto");
            var user = _repository.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (user is null)
                return ResponseFactory.CreateResponseFromResult(false, false, "Non esiste un utente con questo id");
            // arrivato qui sono sicuro che l'utente esiste, e prima di eliminarlo devo eliminare anche i suoi appuntamenti
            foreach (var evento in user.Appuntamenti)
            {
                evento.IsDeleted = true;
            }
            // e devo toglierlo anche dalle partecipazioni
            foreach (var partecipante in user.Partecipazioni)
            {
                user.Partecipazioni.Remove(partecipante);
                partecipante.IsDeleted = true;
            }
            user.IsDeleted = true;
            var result = _repository.SaveChanges();
            return ResponseFactory.CreateResponseFromResult(result);
        }
    }
}
