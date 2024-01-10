using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
using ProgettoParadigmi.Models.Dto;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class NotificationPageViewModel(IAppuntamentiService service) : BaseViewModel
{
    public ObservableCollection<AppuntamentoDaAccettareDto> AppuntamentiDaAccettare { get; } = new();


    [RelayCommand]
    private async Task LoadItems()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            AppuntamentiDaAccettare.Clear();
            var events = await service.GetAppuntamentiDaAccettare(App.UserDetails.Id);
            if (events.IsSuccess)
            {
                foreach (var e in events.Result)
                {
                    AppuntamentiDaAccettare.Add(e);
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Errore nel caricamento utenti: {e.Message}");
            await Shell.Current.DisplayAlert("Errore", e.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AggiornaStatoInvito(object[] values)
    {
        if (values.Length > 0)
        {
            IsBusy = true;
            var id = (Guid) values[0];
            var accetta = (bool)values[1];
            var stato = accetta ? StatoInvito.Accettato : StatoInvito.Rifiutato;
            var dto = new AggiornaStatoInvitoDto { Stato = stato, PartecipazioneId = id };
            var response = await service.AggiornaStatoInvito(dto);
            if (response.IsSuccess)
            {
                var item = AppuntamentiDaAccettare.FirstOrDefault(x => x.Id == id);
                if (item is not null)
                {
                    AppuntamentiDaAccettare.Remove(item);
                }

                IsBusy = false;
            }
            else
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("Errore", response.Error, "Ok");
            }
        }
    }
}