using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Mobile.Views.Startup;
using ProgettoParadigmi.Models.Dto;
using ProgettoParadigmi.Models.Entities;
using UraniumUI.Dialogs;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class ProfilePageViewModel(IUserService service, IDialogService dialogService) : BaseViewModel
{
    [ObservableProperty, NotifyPropertyChangedFor(nameof(IsEditable), nameof(IsCategoriaEditable))] private AuthDto? _userDetails;
    [ObservableProperty] private string _initials = "";
    [ObservableProperty] private Color _avatarBg;
    [ObservableProperty] private Color _avatarTextColor;
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

    public bool IsCategoriaEditable
    {
        get
        {
            if (PreviousPage == "None")
            {
                return false;
            }
            if (UserDetails != null && App.UserDetails != null)
            {
                return App.UserDetails.TipoUtente == TipoUtente.Admin;
            }

            return false;
        }
    }


    public void LoadProperties()
    {
        if (UserDetails is null) return;
        Initials = $"{UserDetails.Nome[0]}{UserDetails.Cognome[0]}";
        AvatarBg = ColorGenerator.GenerateRandomColor();
        AvatarTextColor = ColorGenerator.GetTextColor(AvatarBg);
        TipoUtenteString = UserDetails.TipoUtente.ToString();
        
    }

    [RelayCommand]
    private async Task GoToHome() => await Shell.Current.GoToAsync($"//{nameof(HomePage)}?Categoria={Guid.Empty.ToString()}");

    [RelayCommand]
    private async Task ChangeTipoUtente()
    {
        if (IsCategoriaEditable)
        {
            var result = await dialogService.DisplayRadioButtonPromptAsync(
                "Scegli un tipo utente", Enum.GetNames<TipoUtente>(), TipoUtenteString, "Ok", "Annulla");
            if (Enum.TryParse(result, out TipoUtente tipo))
            {
                TipoUtenteString = result;
                UserDetails = new AuthDto(UserDetails.Nome, UserDetails.Cognome, UserDetails.Email, UserDetails.Id, tipo);
            }
        }
    }

    [RelayCommand]
    private async Task SalvaUtente()
    {
        try
        {
            IsBusy = true;
            var user = new UtenteDto(UserDetails.Id, UserDetails.Nome, UserDetails.Cognome, UserDetails.Email,
                UserDetails.TipoUtente);
            var result = await service.SaveUser(user);
            if (result is { IsSuccess: true, Result: true })
            {
                await Shell.Current.DisplayAlert("", "Utente salvato con successo", "Ok");
                if (UserDetails.Id == App.UserDetails.Id)
                {
                    App.UserDetails = UserDetails;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Errore", result.Error, "Ok");
            }
        }
        catch (Exception e)
        {
            await Shell.Current.DisplayAlert("Errore", e.Message, "Ok");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AggiornaPassword()
    {
        var result = await
            dialogService.DisplayTextPromptAsync("Cambia password", "Inserisci la nuova password", "Ok", "Annulla");
        if (!string.IsNullOrEmpty(result))
        {
            try
            {
                IsBusy = true;
                var dto = new ChangePasswordDto { Id = UserDetails.Id, Password = result };
                var response = await service.UpdatePassword(dto);
                if (response is { IsSuccess: true, Result: true })
                {
                    await Shell.Current.DisplayAlert("", "Password salvata con successo", "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Errore", response.Error, "Ok");
                }
            }
            catch (Exception e)
            {
                await Shell.Current.DisplayAlert("Errore", e.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

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