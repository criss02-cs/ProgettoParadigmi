using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
using ProgettoParadigmi.Models.Dto;

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
}