using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;
[QueryProperty(nameof(Categoria), nameof(Categoria))]
public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel vm)
    {
        InitializeComponent();
        if (!string.IsNullOrEmpty(_categoria))
        {
            vm.CategoriaId = Guid.Parse(_categoria);
        }
        BindingContext = vm;
    }

    private string _categoria = "";

    public string Categoria
    {
        set
        {
            _categoria = value;
            if (BindingContext is not HomePageViewModel vm) return;
            vm.CategoriaId = Guid.Parse(_categoria);
            Shell.Current.FlyoutIsPresented = false;
            vm.LoadEventsCommand.ExecuteAsync(Tuple.Create(DateTime.Now.Month, DateTime.Now.Year));
        }
    }

    private void HomePage_OnAppearing(object? sender, EventArgs e)
    {
        if (BindingContext is HomePageViewModel vm)
        {
            vm.LoadEventsCommand.ExecuteAsync(Tuple.Create(DateTime.Now.Month, DateTime.Now.Year));
        }
    }
}