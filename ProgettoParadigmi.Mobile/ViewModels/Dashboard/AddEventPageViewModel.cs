using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Mobile.Views.Popups;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class AddEventPageViewModel : BaseViewModel
{
    [ObservableProperty] private AppuntamentoDto _appuntamentoDto = new()
    {
        OrganizzatoreId = App.UserDetails.Id,
        DataInizio = DateTime.Now,
        DataFine = DateTime.Now.AddHours(1),
        Partecipanti = []
    };
    private readonly IAppuntamentiService _service;
    private readonly IUserService _userService;
    [ObservableProperty] private DateTime _dataFine = DateTime.Now.AddHours(1);
    [ObservableProperty] private List<CategoriaDto> _categorie = App.Categorie;
    [ObservableProperty] private CategoriaDto _categoriaSelezionata = new("", Guid.Empty, "");
    public bool IsPartecipantiVisible => Partecipanti.Count > 0;
    public ObservableCollection<UtenteDto> Partecipanti { get; } = new();

    public AddEventPageViewModel(IAppuntamentiService service, IUserService userService)
    {
        _service = service;
        _userService = userService;
    }


    [RelayCommand]
    public async Task CreateEvent()
    {
        if (CategoriaSelezionata.Id == Guid.Empty)
        {
            await Shell.Current.DisplayAlert("Errore", "Si prega di selezionare una categoria", "Ok");
            return;
        }
        AppuntamentoDto.Categoria = CategoriaSelezionata;
        AppuntamentoDto.Partecipanti = Partecipanti.Select(x => x.Id).ToList();
        var result = await _service.CreaAppuntamento(AppuntamentoDto);
        if (result is { IsSuccess: true })
        {
            await Shell.Current.DisplayAlert("", "Appuntamento creato con successo!", "Ok");
            // torna alla pagina precedente
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Errore", result.Error, "Ok");
        }
    }

    [RelayCommand]
    private async Task OpenUserList()
    {
        var popup = new UserListPopupPage(_userService, Partecipanti.ToList());
        await MopupService.Instance.PushAsync(popup);
        var result = await popup.PopupDismissedTask;
        if (result.Count > 0)
        {
            foreach (var utente in result)
            {
                Partecipanti.Add(utente);
            }
            OnPropertyChanged(nameof(IsPartecipantiVisible));
        }
    }
}