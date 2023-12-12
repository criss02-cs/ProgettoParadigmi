using ProgettoParadigmi.Mobile.Controls;
using ProgettoParadigmi.Mobile.FontModels;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Mobile.Utils;

public static class FlyoutManager
{
    public static async Task AddFlyoutMenusDetails()
    {
        AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
        
        // rimuovi tutte le pagine
        
        
        // aggiungi le pagine in base al tipo utente
        // se è admin avrà anche una pagina: admin dashboard
        var flyoutItem = new FlyoutItem
        {
            Title = "Home",
            Route = nameof(HomePage),
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
            Items =
            {
                new ShellContent
                {
                    Icon = new FontImageSource
                    {
                        FontFamily = "FaSolid",
                        Glyph = FaSolidIcons.House,
                        Color = Colors.White
                    },
                    Title = "Home",
                    ContentTemplate = new DataTemplate(typeof(HomePage))
                }
            }
        };
        Routing.RegisterRoute(nameof(AddEventPage), typeof(AddEventPage));
        if (!AppShell.Current.Items.Contains(flyoutItem))
        {
            AppShell.Current.Items.Add(flyoutItem);
        }

        // if (App.UserDetails.TipoUtente == TipoUtente.Admin)
        // {
        //     // aggiungo la pagina di login
        //     flyoutItem = new FlyoutItem
        //     {
        //         Title = "Admin dashboard",
        //         Route = "AdminDashboardPage",
        //         FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
        //         Items = { }
        //     };
        //     if (!AppShell.Current.Items.Contains(flyoutItem))
        //     {
        //         AppShell.Current.Items.Add(flyoutItem);
        //     }
        // }

        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
}