using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Views.Startup;

namespace ProgettoParadigmi.Mobile.ViewModels;

public partial class AppShellViewModel : BaseViewModel
{
    [RelayCommand]
    public async Task SignOut()
    {
        if (Preferences.ContainsKey(nameof(App.UserDetails)))
        {
            Preferences.Remove(nameof(App.UserDetails));
        }
        SecureStorage.Remove("cred");
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}