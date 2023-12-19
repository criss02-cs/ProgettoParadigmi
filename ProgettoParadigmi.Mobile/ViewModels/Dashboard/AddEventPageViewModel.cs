using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
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
    private IAppuntamentiService _service;
    [ObservableProperty] private DateTime _dataFine = DateTime.Now.AddHours(1);
    [ObservableProperty] private List<CategoriaDto> _categorie = App.Categorie;
    [ObservableProperty] private CategoriaDto _categoriaSelezionata = new("", Guid.Empty, "");

    public AddEventPageViewModel(IAppuntamentiService service)
    {
        _service = service;
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