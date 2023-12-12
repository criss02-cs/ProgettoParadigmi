using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Mobile.Views.Startup;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Startup;

public class LoadingPageViewModel
{
    public LoadingPageViewModel()
    {
        CheckUserLoginDetails();
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
                await Shell.Current.DisplayAlert("Sessione scaduta", "Effettua di nuovo l'accesso per continuare",
                    "Ok");
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                var userInfo = JsonSerializer.Deserialize<AuthDto>(userDetailsStr);
                App.UserDetails = userInfo;
                App.Token = tokenDetails;
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