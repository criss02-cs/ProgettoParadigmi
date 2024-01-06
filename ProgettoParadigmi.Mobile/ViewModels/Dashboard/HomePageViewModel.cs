using System.Diagnostics;
using System.Globalization;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Calendar.Enums;
using Plugin.Maui.Calendar.Models;
using ProgettoParadigmi.Mobile.Models;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class HomePageViewModel : BaseViewModel
{
    private readonly IAppuntamentiService _service;
    [ObservableProperty] private CultureInfo _culture;
    [ObservableProperty] private WeekLayout _layout;
    [ObservableProperty] private DateTime _shownDate = DateTime.Today;
    [ObservableProperty] private EventCollection _events;
    [ObservableProperty] private DateTime? _selectedDate;
    public Guid CategoriaId { get; set; } = Guid.Empty; 

    public HomePageViewModel(IAppuntamentiService service)
    {
        _service = service;
        Culture = CultureInfo.CreateSpecificCulture("it-IT");
        Events = new EventCollection();
        SelectedDate = DateTime.Today;
        Layout = WeekLayout.Month;
        LoadEvents(Tuple.Create(DateTime.Today.Month, DateTime.Today.Year));
    }
    [RelayCommand]
    public async Task LoadEvents(Tuple<int, int> data)
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            Events.Clear();
            var events =
                await _service.GetAppuntamentiByUserId(App.UserDetails.Id, data.Item1, data.Item2);
            switch (events.IsSuccess)
            {
                case true when events.Result.Count > 0:
                {
                    // Events.Clear();
                    if (CategoriaId != Guid.Empty)
                    {
                        events.Result = events.Result
                            .Where(x => x.Categoria.Id == CategoriaId)
                            .ToList();
                    }

                    var grouped = events.Result
                        .GroupBy(x => x.DataInizio)
                        .ToList();
                    foreach (var g in grouped)
                    {
                        Events.Add(g.Key, g.ToDayEventCollection());
                    }
                    break;
                }
                case false:
                    await Shell.Current.DisplayAlert("Errore", events.Error, "Ok");
                    break;
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
    public async Task CreateNewEvent()
    {
        await Shell.Current.GoToAsync(nameof(AddEventPage), true);
    }

    [RelayCommand]
    public async Task SwipeLeft() => await ChangeShownUnit(1);

    [RelayCommand]
    public async Task SwipeRight() => await ChangeShownUnit(-1);

    [RelayCommand]
    public void SwipeUp() => ShownDate = DateTime.Today;

    [RelayCommand]
    public async Task EventSelected(object item)
    {
        if (item is AdvancedEventModel eventModel)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Appuntamento", eventModel }
            };
            await Shell.Current.GoToAsync($"{nameof(EventDetailsPage)}", true, parameters);
        }
    }

    [RelayCommand]
    private async Task GoToNotifications() => await Shell.Current.GoToAsync($"NotificationPage");

    private async Task ChangeShownUnit(int amountToAdd)
    {
        switch (Layout)
        {
            case WeekLayout.Week:
            case WeekLayout.TwoWeek:
                ChangeShownWeek(amountToAdd);
                break;
            case WeekLayout.Month:
            default:
                ChangeShownMonth(amountToAdd);
                break;
        }
        await LoadEvents(Tuple.Create(ShownDate.Month, ShownDate.Year));
    }

    private void ChangeShownMonth(int monthsToAdd) => ShownDate = ShownDate.AddMonths(monthsToAdd);

    private void ChangeShownWeek(int weeksToAdd) => ShownDate = ShownDate.AddDays(weeksToAdd * 7);
}