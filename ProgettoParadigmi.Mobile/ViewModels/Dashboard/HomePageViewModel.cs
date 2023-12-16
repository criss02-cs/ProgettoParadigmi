using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Calendar.Enums;
using Plugin.Maui.Calendar.Models;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
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

    public HomePageViewModel(IAppuntamentiService service)
    {
        _service = service;
        Culture = CultureInfo.CreateSpecificCulture("it-IT");
        Events = new EventCollection();
        SelectedDate = DateTime.Today;
        LoadEvents(DateTime.Today.Month, DateTime.Today.Year);
    }

    private async Task LoadEvents(int month, int year)
    {
        var events =
            await _service.GetAppuntamentiByUserId(App.UserDetails.Id, month, year);
        switch (events.IsSuccess)
        {
            case true when events.Result.Count > 0:
            {
                var grouped = events.Result
                    .GroupBy(x => x.DataInizio)
                    .ToList();
                foreach (var g in grouped)
                {
                    Events.Add(g.Key, g.ToList());
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
        await LoadEvents(ShownDate.Month, ShownDate.Year);
    }

    private void ChangeShownMonth(int monthsToAdd) => ShownDate.AddMonths(monthsToAdd);

    private void ChangeShownWeek(int weeksToAdd) => ShownDate.AddDays(weeksToAdd * 7);
}