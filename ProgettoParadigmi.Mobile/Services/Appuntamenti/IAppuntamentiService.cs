using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Services.Appuntamenti;

public interface IAppuntamentiService
{
    Task<Response<List<AppuntamentoDto>>?> GetAppuntamentiByUserId(Guid userId, int mese, int anno);
    Task<Response<bool>?> CreaAppuntamento(AppuntamentoDto dto);
    Task<Response<List<AppuntamentoDaAccettareDto>>> GetAppuntamentiDaAccettare(Guid userId);
}