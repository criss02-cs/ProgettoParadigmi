using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.Views.Dashboard;

namespace ProgettoParadigmi.Mobile.Controls;

public partial class FlyoutFooterControl : StackLayout
{
    public FlyoutFooterControl()
    {
        InitializeComponent();
    }

    private async void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        Shell.Current.FlyoutIsPresented = false;
        await Shell.Current.GoToAsync($"{nameof(ProfilePage)}");
    }
}