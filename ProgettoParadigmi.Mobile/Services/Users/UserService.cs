using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Services.Users;

public class UserService : IUserService
{
    private string _utentiAddress = $"{Costanti.BaseAddress}/api/Utenti";
    
    public async Task<Response<List<UtenteDto>>> GetAllUsers(int take = 10, int skip = 0)
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.GetAsync($"{_utentiAddress}/GetAll/{take}/{skip}");
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Response<List<UtenteDto>>>(content, Costanti.DefaultOptions);
                return result;
            }

            var error = await response.Content.ReadAsStringAsync();
            return ResponseFactory.CreateResponseFromResult<List<UtenteDto>>(null, false, error);
        }
    }

    public async Task<Response<AuthDto>> InsertNewUser(RegisterDto user)
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.PostAsJsonAsync($"{_utentiAddress}/Insert", user);
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Response<AuthDto>>(content, Costanti.DefaultOptions);
                return result;
            }
            var error = await response.Content.ReadAsStringAsync();
            return ResponseFactory.CreateResponseFromResult<AuthDto>(null, false, error);
        }
    }

    public async Task<Response<bool>> EliminaUtente(Guid id)
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.GetAsync($"{_utentiAddress}/Delete/{id}");
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
}