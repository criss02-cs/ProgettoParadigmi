using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.Models;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;

[QueryProperty(nameof(Appuntamento), nameof(Appuntamento))]
public partial class EventDetailsPage : ContentPage
{
    public EventDetailsPage(EventDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    public AdvancedEventModel Appuntamento
    {
        set
        {
            if (BindingContext is not EventDetailsViewModel vm) return;
            vm.Appuntamento = value;
        }
    }
}