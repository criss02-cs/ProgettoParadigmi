using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Mobile.Views.Popups;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class AdminPageViewModel(IUserService service) : BaseViewModel
{
    public ObservableCollection<UtenteDto> Users { get; set; } = [];
    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private string _filtroRicerca;

    [RelayCommand]
    private async Task GoToDetail(UtenteDto utente)
    {
        var dto = new AuthDto(utente.Nome, utente.Cognome, utente.Email, utente.Id, utente.TipoUtente);
        var parameters = new Dictionary<string, object>
        {
            { "UserDetails", dto },
            { "PreviousPage", "AdminPage" }
        };
        await Shell.Current.GoToAsync($"{nameof(ProfilePage)}", true, parameters);
    }

    [RelayCommand]
    private async Task SearchUser()
    {
        await LoadUsers(Tuple.Create(10, 0, FiltroRicerca));
        // Keyboard.
    }

    [RelayCommand]
    private async Task AddNewUser()
    {
        var popup = new NewUserPopupPage();
        await MopupService.Instance.PushAsync(popup);
        var user = await popup.PopupDismissedTask;
        // faccio la validazione anche qui perché quando clicco al di fuori del popup
        // lui mi ritorna lo user vuoto
        if (ValidateUser(user))
        {
            var result = await service.InsertNewUser(user);
            if (result.IsSuccess)
            {
                await Shell.Current.DisplayAlert("", "Utente salvato con successo!", "Ok");
                await LoadUsers();
            }
            else
            {
                await Shell.Current.DisplayAlert("Errore", result.Error, "Ok");
            }
        }
    }

    private static bool ValidateUser(RegisterDto user)
    {
        var validationContext = new ValidationContext(user);
        var validationResults = new List<ValidationResult>();
        return Validator.TryValidateObject(user, validationContext, validationResults);
    }

    [RelayCommand]
    private async Task LoadUsers(Tuple<int, int, string>? filtri = null)
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            var response = await service.GetAllUsers(filtri?.Item1 ?? 10, filtri?.Item2 ?? 0, filtri?.Item3 ?? "");
            if (Users.Count != 0)
            {
                Users.Clear();
            }

            if (response is { IsSuccess: true, Result: not null })
            {
                foreach (var user in response.Result.OrderBy(x => x.Nome))
                {
                    Users.Add(user);
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Errore", response.Error, "Ok");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Errore nel caricamento utenti: {ex.Message}");
            await Shell.Current.DisplayAlert("Errore", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
}