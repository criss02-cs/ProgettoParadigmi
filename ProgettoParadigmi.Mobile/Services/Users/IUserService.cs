using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Services.Users;

public interface IUserService
{
    Task<Response<List<UtenteDto>>> GetAllUsers(int take = 10, int skip = 0);
}