using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Models.Dto;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class ProfilePageViewModel : BaseViewModel
{
    [ObservableProperty, NotifyPropertyChangedFor(nameof(IsEditable))] private AuthDto? _userDetails;
    [ObservableProperty] private string _initials = "";
    [ObservableProperty] private Color _avatarBg;
    [ObservableProperty] private string _tipoUtenteString = "";

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
}