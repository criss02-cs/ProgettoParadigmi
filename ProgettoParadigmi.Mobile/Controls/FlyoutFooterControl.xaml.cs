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
        var utente = App.UserDetails;
        if (utente == null) return;
        var parameters = new Dictionary<string, object>
        {
            { "UserDetails", utente },
            { "PreviousPage", "None" }
        };
        await Shell.Current.GoToAsync($"{nameof(ProfilePage)}", true, parameters);

    }
}