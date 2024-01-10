using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;

public partial class AddEventPage : ContentPage
{
    public AddEventPage(AddEventPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void AddEventPage_OnAppearing(object? sender, EventArgs e)
    {
        if (BindingContext is not AddEventPageViewModel vm) return;
        vm.AppuntamentoDto = new AppuntamentoDto
        {
            OrganizzatoreId = App.UserDetails.Id,
            DataInizio = DateTime.Now,
            DataFine = DateTime.Now.AddHours(1),
            Partecipanti = []
        };
    }
}