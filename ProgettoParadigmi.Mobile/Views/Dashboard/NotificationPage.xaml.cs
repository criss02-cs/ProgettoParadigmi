using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;

public partial class NotificationPage : ContentPage
{
    public NotificationPage(NotificationPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void NotificationPage_OnAppearing(object? sender, EventArgs e)
    {
        if (BindingContext is not NotificationPageViewModel vm) return;
        vm.LoadItemsCommand.ExecuteAsync(null);
    }
}