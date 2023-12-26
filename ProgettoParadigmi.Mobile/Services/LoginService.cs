using System.Net;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Services;

public class LoginService : ILoginService
{
    private readonly string _authAddress = $"{Costanti.BaseAddress}/api/Auth";
    public async Task<Response<AuthDto>?> Authenticate(LoginDto request)
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.PostAsJsonAsync($"{_authAddress}/Login", request);
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

    public async Task<Response<AuthDto>?> Register(RegisterDto request)
    {
        using (var client = HttpClientFactory.Create())
        {
            var response = await client.PostAsJsonAsync($"{_authAddress}/Register", request);
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
}