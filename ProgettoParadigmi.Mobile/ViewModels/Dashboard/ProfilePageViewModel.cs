using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Mobile.Views.Startup;
using ProgettoParadigmi.Models.Dto;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class ProfilePageViewModel(IUserService service) : BaseViewModel
{
    [ObservableProperty, NotifyPropertyChangedFor(nameof(IsEditable))] private AuthDto? _userDetails;
    [ObservableProperty] private string _initials = "";
    [ObservableProperty] private Color _avatarBg;
    [ObservableProperty] private string _tipoUtenteString = "";
    [ObservableProperty] private string _previousPage = "";

    public bool IsEditable
    {
        get
        {
            if (UserDetails != null && App.UserDetails != null)
            {
                return App.UserDetails.TipoUtente == TipoUtente.Admin || App.UserDetails.Id == UserDetails.Id;
            }

            return false;
        }
    }


    public void LoadProperties()
    {
        if (UserDetails is null) return;
        Initials = $"{UserDetails.Nome[0]}{UserDetails.Cognome[0]}";
        AvatarBg = ColorGenerator.GenerateRandomColor();
        TipoUtenteString = UserDetails.TipoUtente.ToString();
        
    }

    [RelayCommand]
    private async Task GoToHome() => await Shell.Current.GoToAsync($"//{nameof(HomePage)}?Categoria={Guid.Empty.ToString()}");

    [RelayCommand]
    private async Task EliminaUtente()
    {
        // if (PreviousPage == "None")
        // {
        //     
        // }
        var result = await Shell.Current.DisplayAlert("Eliminazione account",
            "Sei sicuro di voler eliminare questo account?", "Sì", "No");
        if (result)
        {
            var isDelete = await service.EliminaUtente(UserDetails.Id);
            if (isDelete.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Eliminazione account", "Account eliminato correttamente!", "Ok");
                // se provengo dalla schermata profilo allora torno alla pagina login
                if (PreviousPage == "None")
                {
                    if (Preferences.ContainsKey(nameof(App.UserDetails)))
                    {
                        Preferences.Remove(nameof(App.UserDetails));
                    }
                    SecureStorage.Remove("cred");
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
                // altrimenti torno indietro
                else
                {
                    Shell.Current.SendBackButtonPressed();
                }
            }
        }
    }
}