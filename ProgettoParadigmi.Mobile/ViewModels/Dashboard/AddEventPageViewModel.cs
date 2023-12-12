using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class AddEventPageViewModel(IAppuntamentiService service) : BaseViewModel
{
    [ObservableProperty] private AppuntamentoDto _appuntamentoDto = new()
    {
        OrganizzatoreId = App.UserDetails.Id,
        DataInizio = DateTime.Now,
        DataFine = DateTime.Now.AddHours(1)
    };
    private IAppuntamentiService _service = service;


    [RelayCommand]
    public async Task CreateEvent()
    {
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
}