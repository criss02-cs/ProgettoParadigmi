using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class AdminPageViewModel(IUserService service) : BaseViewModel
{
    public ObservableCollection<UtenteDto> Users { get; set; } = [];
    [ObservableProperty] private bool _isRefreshing;

    [RelayCommand]
    private async Task LoadUsers()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            var response = await service.GetAllUsers();
            if (Users.Count != 0)
            {
                Users.Clear();
            }

            if (response is { IsSuccess: true, Result: not null })
            {
                foreach (var user in response.Result)
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