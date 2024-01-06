using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;

public partial class AdminPage : ContentPage
{
    public AdminPage(AdminPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void AdminPage_OnAppearing(object? sender, EventArgs e)
    {
        if (BindingContext is AdminPageViewModel vm)
        {
            vm.LoadUsersCommand.ExecuteAsync(null);
        }
    }
}