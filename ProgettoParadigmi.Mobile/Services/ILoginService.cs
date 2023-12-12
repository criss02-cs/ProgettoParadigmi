using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Services;

public interface ILoginService
{
    Task<Response<AuthDto>?> Authenticate(LoginDto request);
}