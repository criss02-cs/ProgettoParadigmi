using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Services.Appuntamenti;

public class AppuntamentiService : IAppuntamentiService
{
    private readonly string _appuntamentiUrl = $"{Costanti.BaseAddress}/api/Appuntamenti";
    public async Task<Response<List<AppuntamentoDto>>?> GetAppuntamentiByUserId(Guid userId, int mese, int anno)
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.GetAsync($"{_appuntamentiUrl}/{userId}/{mese}/{anno}");
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Response<List<AppuntamentoDto>>>(content, Costanti.DefaultOptions);
                return result;
            }
            var error = await response.Content.ReadAsStringAsync();
            return ResponseFactory.CreateResponseFromResult<List<AppuntamentoDto>>([], false, error);
        }
    }

    public async Task<Response<bool>?> CreaAppuntamento(AppuntamentoDto dto)
    {
        try
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = await client.PostAsJsonAsync($"{_appuntamentiUrl}/Insert", dto);
                if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Response<bool>>(content, Costanti.DefaultOptions);
                    return result;
                }

                var error = await response.Content.ReadAsStringAsync();
                return ResponseFactory.CreateResponseFromResult(false, false, error);
            }
        }
        catch (Exception e)
        {
            return ResponseFactory.CreateResponseFromResult(false, false, e.Message);
        }
    }
}