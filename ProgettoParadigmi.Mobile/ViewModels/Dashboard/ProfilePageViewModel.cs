using CommunityToolkit.Mvvm.ComponentModel;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Models.Dto;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class ProfilePageViewModel : BaseViewModel
{
    [ObservableProperty] private AuthDto _userDetails;
    [ObservableProperty] private string _initials;
    [ObservableProperty] private Color _avatarBg;


    public ProfilePageViewModel()
    {
        UserDetails = App.UserDetails ?? new AuthDto("", "", "", Guid.Empty, TipoUtente.Utente);
        Initials = $"{UserDetails.Nome[0]}{UserDetails.Cognome[0]}";
        AvatarBg = ColorGenerator.GenerateRandomColor();
    }
}