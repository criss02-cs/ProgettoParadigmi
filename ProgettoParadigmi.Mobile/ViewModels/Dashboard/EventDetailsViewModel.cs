using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Models;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class EventDetailsViewModel(IUserService service) : BaseViewModel
{
    [ObservableProperty, NotifyPropertyChangedFor(nameof(DataFine), nameof(IsDataFineVisible), nameof(IsAutore))]
    private AdvancedEventModel _appuntamento;

    public ObservableCollection<CategoriaDto> Categorie { get; } = new(App.Categorie);
    public ObservableCollection<UtenteDto> Invitati { get; set; } = new();
    public DateTime DataFine => Appuntamento?.DataFine ?? DateTime.MinValue;
    public bool IsDataFineVisible => DataFine != DateTime.MinValue;
    public bool IsAutore => Appuntamento?.OrganizzatoreId == App.UserDetails.Id;
    [ObservableProperty] private string _nomeOrganizzatore;

    [RelayCommand]
    private async Task LoadOrganizzatore()
    {
        if (!IsAutore)
        {
            var response = await service.GetById(Appuntamento.OrganizzatoreId);
            if (response.IsSuccess)
            {
                NomeOrganizzatore = $"{response.Result.Nome} {response.Result.Cognome}";
            }
        }
    }
}