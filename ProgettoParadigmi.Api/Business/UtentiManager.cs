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


        public Response<List<UtenteDto>> GetAll(int take = 10, int skip = 0)
        {
            var users = _repository
                .Query(orderBy: x => x.OrderBy(y => y.Cognome))
                .Skip(skip)
                .Take(10)
                .Select(x => new UtenteDto(x.Id, x.Nome, x.Cognome, x.Email, x.TipoUtente))
                .ToList();
            return ResponseFactory.CreateResponseFromResult(users);
        }

        public Response<bool> ChangePassword(ChangePasswordDto dto)
        {
            if (dto.Id == Guid.Empty)
                return ResponseFactory.CreateResponseFromResult(false, false, "L'id non può essere vuoto");
            var user = _repository.GetFirstOrDefault(x => x.Id == dto.Id);
            if(user is null)
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
            var user = _repository.GetFirstOrDefault(x => x.Id == id);
            if (user is null)
                return ResponseFactory.CreateResponseFromResult<UtenteDto>(null, false, "Non esiste un utente con questo id");
            var dto = new UtenteDto(user.Id, user.Nome, user.Cognome, user.Email, user.TipoUtente);
            return ResponseFactory.CreateResponseFromResult(dto);
        }
    }
}
