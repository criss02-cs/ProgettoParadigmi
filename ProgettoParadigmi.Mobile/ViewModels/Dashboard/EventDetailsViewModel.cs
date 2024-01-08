using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ProgettoParadigmi.Mobile.Models;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class EventDetailsViewModel(IAppuntamentiService service) : BaseViewModel
{
    [ObservableProperty, NotifyPropertyChangedFor(nameof(DataFine), nameof(IsDataFineVisible))]
    private AdvancedEventModel _appuntamento;

    public ObservableCollection<CategoriaDto> Categorie { get; } = new(App.Categorie);
    public ObservableCollection<UtenteDto> Invitati { get; set; } = new();
    public DateTime DataFine => Appuntamento?.DataFine ?? DateTime.MinValue;
    public bool IsDataFineVisible => DataFine != DateTime.MinValue;
}