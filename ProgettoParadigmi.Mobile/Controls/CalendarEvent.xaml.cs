using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ProgettoParadigmi.Mobile.Models;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Controls;

public partial class CalendarEvent : ContentView
{
    public static BindableProperty CalendarEventCommandProperty =
        BindableProperty.Create(nameof(CalendarEventCommand), typeof(ICommand), typeof(CalendarEvent), null);

    public CalendarEvent()
    {
        InitializeComponent();
    }

    public ICommand CalendarEventCommand
    {
        get => (ICommand)GetValue(CalendarEventCommandProperty);
        set => SetValue(CalendarEventCommandProperty, value);
    }

    private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
    {
        if (BindingContext is AppuntamentoDto eventModel)
            CalendarEventCommand?.Execute(eventModel);
    }
}