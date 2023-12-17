using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Services.Categorie;

public class CategorieService : ICategorieService
{
    private readonly string _categorieUrl = $"{Costanti.BaseAddress}/api/Categorie";
    public async Task<Response<List<CategoriaDto>>> GetByUserId(Guid userId)
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.GetAsync($"{_categorieUrl}/{userId}");
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Response<List<CategoriaDto>>>(content, Costanti.DefaultOptions);
                return result;
            }
            var error = await response.Content.ReadAsStringAsync();
            return ResponseFactory.CreateResponseFromResult<List<CategoriaDto>>([], false, error);
        }
    }

    public async Task<Response<bool>> Insert(CategoriaDto dto)
    {
        try
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = await client.PostAsJsonAsync($"{_categorieUrl}/Insert", dto);
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
            Console.WriteLine(e);
            return ResponseFactory.CreateResponseFromResult(false, false, e.Message);
        }
    }
}