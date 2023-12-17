using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Services.Categorie;

public interface ICategorieService
{
    Task<Response<List<CategoriaDto>>> GetByUserId(Guid userId);
    Task<Response<bool>> Insert(CategoriaDto dto);
}