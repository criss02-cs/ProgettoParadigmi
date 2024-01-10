using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Services.Users;

public interface IUserService
{
    Task<Response<List<UtenteDto>>> GetAllUsers(int take = 10, int skip = 0, string filtro = "");
    Task<Response<AuthDto>> InsertNewUser(RegisterDto user);
    Task<Response<bool>> EliminaUtente(Guid id);
    Task<Response<UtenteDto>> GetById(Guid id);
    Task<Response<bool>> SaveUser(UtenteDto dto);
    Task<Response<bool>> UpdatePassword(ChangePasswordDto dto);
}