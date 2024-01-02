using System.Globalization;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Calendar.Enums;
using Plugin.Maui.Calendar.Models;
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
        LoadEvents(Tuple.Create(DateTime.Today.Month, DateTime.Today.Year));
    }
    [RelayCommand]
    public async Task LoadEvents(Tuple<int, int> data)
    {
        Events.Clear();
        var events =
            await _service.GetAppuntamentiByUserId(App.UserDetails.Id, data.Item1, data.Item2);
        switch (events.IsSuccess)
        {
            case true when events.Result.Count > 0:
            {
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
        if (item is AppuntamentoDto eventModel)
        {
            var title = $"Selected: {eventModel.Titolo}";
            var message =
                $"Starts: {eventModel.DataInizio:HH:mm}{Environment.NewLine}Finishes: {eventModel.DataFine:HH:mm}{Environment.NewLine}Details: {eventModel.Descrizione}";
            await App.Current.MainPage.DisplayAlert(title, message, "Ok");
        }
    }

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

    private void ChangeShownMonth(int monthsToAdd) => ShownDate.AddMonths(monthsToAdd);

    private void ChangeShownWeek(int weeksToAdd) => ShownDate.AddDays(weeksToAdd * 7);
}