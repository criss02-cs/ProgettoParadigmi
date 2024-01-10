using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Services.Users;

public class UserService : IUserService
{
    private string _utentiAddress = $"{Costanti.BaseAddress}/api/Utenti";
    
    public async Task<Response<List<UtenteDto>>> GetAllUsers(int take = 10, int skip = 0, string filtro = "")
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.GetAsync($"{_utentiAddress}/GetAll/{take}/{skip}/{filtro}");
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

    public async Task<Response<UtenteDto>> GetById(Guid id)
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.GetAsync($"{_utentiAddress}/{id}");
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Response<UtenteDto>>(content, Costanti.DefaultOptions);
                return result;
            }

            var error = await response.Content.ReadAsStringAsync();
            return ResponseFactory.CreateResponseFromResult<UtenteDto>(null, false, error);
        }
    }

    public async Task<Response<bool>> SaveUser(UtenteDto dto)
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.PostAsJsonAsync($"{_utentiAddress}/Update", dto);
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

    public async Task<Response<bool>> UpdatePassword(ChangePasswordDto dto)
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.PostAsJsonAsync($"{_utentiAddress}/ChangePassword", dto);
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