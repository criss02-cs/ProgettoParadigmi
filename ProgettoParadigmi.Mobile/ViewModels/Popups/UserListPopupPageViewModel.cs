using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Popups;

public partial class UserListPopupPageViewModel : BaseViewModel
{
    public ObservableCollection<UtenteDto> Users { get; } = new();
    // [NotifyPropertyChangedFor(nameof(IsCheckVisible))]
    public ObservableCollection<UtenteDto> SelectedUsers { get; private set; } = new();
    public bool IsCheckVisible => SelectedUsers.Count > 0;
    private IUserService _service;

    public UserListPopupPageViewModel(IUserService service, List<UtenteDto> partecipanti)
    {
        _service = service;
        foreach (var utenteDto in partecipanti)
        {
            SelectedUsers.Add(utenteDto);
        }
        LoadUsers();
    }

    [RelayCommand]
    private void SelectionChanged(object items)
    {
        var collection = items as CollectionView;
        SelectedUsers.Clear();
        foreach (var item in collection.SelectedItems)
        {
            if (item is UtenteDto utente)
            {
                SelectedUsers.Add(utente);
            }
        }
        OnPropertyChanged(nameof(IsCheckVisible));
        // SelectedUsers.AddRange(item)   
    }

    [RelayCommand]
    private void ClearSelectedUsers() => SelectedUsers.Clear();

    [RelayCommand]
    private async Task Select() => await MopupService.Instance.PopAsync();

    [RelayCommand]
    private async Task LoadUsers()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            var response = await _service.GetAllUsers();
            if (response.IsSuccess)
            {
                Users.Clear();
                foreach (var utente in response.Result.Where(x => x.Id != App.UserDetails.Id).OrderBy(x => x.Nome))
                {
                    Users.Add(utente);
                }

                if (Users.Count > 0 && SelectedUsers.Count > 0)
                {
                    var users = Users.Where(x => SelectedUsers.ToList().Exists(y => y.Id == x.Id)).ToList();
                    SelectedUsers = new ObservableCollection<UtenteDto>(users);
                }
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