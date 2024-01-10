using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using ProgettoParadigmi.Mobile.Services;
using ProgettoParadigmi.Mobile.Services.Categorie;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Mobile.Views.Startup;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Startup;

public class LoadingPageViewModel
{
    private readonly ICategorieService _service;
    public LoadingPageViewModel(ICategorieService service)
    {
        CheckUserLoginDetails();
        _service = service;
    }
    private async Task CheckUserLoginDetails()
    {
        var userDetailsStr = Preferences.Get(nameof(App.UserDetails), "");
        if (!string.IsNullOrWhiteSpace(userDetailsStr))
        {
            var tokenDetails = await SecureStorage.GetAsync(nameof(App.Token));
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(tokenDetails) as JwtSecurityToken;
            if (jsonToken.ValidTo < DateTime.UtcNow)
            {
                Routing.UnRegisterRoute(nameof(AddEventPage));
                // await Shell.Current.DisplayAlert("Sessione scaduta", "Effettua di nuovo l'accesso per continuare",
                //     "Ok");
                await ToastService.ShowToast("Sessione scaduta, effettua di nuovo l'accesso per continuare");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                var userInfo = JsonSerializer.Deserialize<AuthDto>(userDetailsStr);
                App.UserDetails = userInfo;
                App.Token = tokenDetails;
                var response = await _service.GetByUserId(App.UserDetails.Id);
                if (response is { IsSuccess: true, Result: not null })
                {
                    App.Categorie = response.Result;
                }
                // TODO aggiungi flyout menu
                await FlyoutManager.AddFlyoutMenusDetails();
            }
        }
        else
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}"); 
        }
    }
}