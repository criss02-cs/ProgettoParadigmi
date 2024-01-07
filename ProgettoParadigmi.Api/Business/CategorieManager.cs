using ProgettoParadigmi.Api.Utils;
using ProgettoParadigmi.Models.Context;
using ProgettoParadigmi.Models.Dto;
using ProgettoParadigmi.Models.Entities;
using ProgettoParadigmi.Models.GenericRepository;

namespace ProgettoParadigmi.Api.Business;

public class CategorieManager(AppuntamentiDbContext ctx)
{
    private IGenericRepository<Categoria> _repository = new GenericRepository<Categoria>(ctx);

    public Response<List<CategoriaDto>> GetCategorieByUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            return ResponseFactory.CreateResponseFromResult<List<CategoriaDto>>([], false,
                "L'id non può essere vuoto");
        var categorie = _repository
            .Query(x => x.User.Id == userId && !x.IsDeleted)
            .Select(x => new CategoriaDto(x.Descrizione, x.Id, x.Color))
            .ToList();
        return ResponseFactory.CreateResponseFromResult(categorie);
    }

    public Response<bool> AddCategoria(CategoriaDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        // controllo che non ci sia una categoria con la stessa descrizione
        // e creata dallo stesso utente
        var isDuplicated = _repository
            .GetFirstOrDefault(x => x.User.Id == dto.UserId && x.Descrizione == dto.Descrizione && !x.IsDeleted);
        if (isDuplicated != null)
        {
            return ResponseFactory
                .CreateResponseFromResult(false, false, "E' già presente una categoria con questa descrizione");
        }
        var categoria = new Categoria
        {
            Descrizione = dto.Descrizione,
            Color = ColorUtils.GetRandomColor(),
            UserId = dto.UserId,
        };
        _repository.Insert(categoria);
        var result = _repository.SaveChanges();
        return ResponseFactory.CreateResponseFromResult(result);
    }
}