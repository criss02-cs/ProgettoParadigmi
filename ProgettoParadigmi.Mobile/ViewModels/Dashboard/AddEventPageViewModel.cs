using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Mobile.Views.Popups;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class AddEventPageViewModel(IAppuntamentiService service, IUserService userService) : BaseViewModel
{
    [ObservableProperty] private AppuntamentoDto _appuntamentoDto = new();
    

    [ObservableProperty] private DateTime _dataFine = DateTime.Now.AddHours(1);
    [ObservableProperty] private DateTime _dataInizio = DateTime.Now.Date;
    [ObservableProperty] private TimeSpan _oraInizio = DateTime.Now.TimeOfDay;
    [ObservableProperty] private TimeSpan _oraFine = DateTime.Now.AddHours(1).TimeOfDay;
    [ObservableProperty] private List<CategoriaDto> _categorie = App.Categorie;
    [ObservableProperty] private CategoriaDto _categoriaSelezionata = new("", Guid.Empty, "");
    public bool IsPartecipantiVisible => Partecipanti.Count > 0;
    public ObservableCollection<UtenteDto> Partecipanti { get; } = new();


    [RelayCommand]
    public async Task CreateEvent()
    {
        if (CategoriaSelezionata is null || CategoriaSelezionata.Id == Guid.Empty)
        {
            await Shell.Current.DisplayAlert("Errore", "Si prega di selezionare una categoria", "Ok");
            return;
        }

        IsBusy = true;
        AppuntamentoDto.Categoria = CategoriaSelezionata;
        AppuntamentoDto.Partecipanti = Partecipanti.ToList();
        AppuntamentoDto.DataInizio = DataInizio.Add(OraInizio);
        AppuntamentoDto.DataFine = DataFine.Add(OraFine);
        var result = await service.CreaAppuntamento(AppuntamentoDto);
        IsBusy = false;
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
        var popup = new UserListPopupPage(userService, Partecipanti.ToList());
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