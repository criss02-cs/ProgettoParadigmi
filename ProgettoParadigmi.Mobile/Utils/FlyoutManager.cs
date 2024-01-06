using CommunityToolkit.Mvvm.Input;
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
        AppShell.Current.FlyoutFooter = new FlyoutFooterControl();
        // rimuovo tutte le pagine
        // AppShell.Current.Items.Clear();

        // aggiungi le pagine in base al tipo utente
        // se è admin avrà anche una pagina: admin dashboard
        AddHomePage();
        // registro tutte le route che non stanno sul menù a comparsa
        Routing.RegisterRoute(nameof(AddEventPage), typeof(AddEventPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(EventDetailsPage), typeof(EventDetailsPage));
        Routing.RegisterRoute(nameof(NotificationPage), typeof(NotificationPage));

        // Carico anche tutte le categorie, e le mostro sul menù a comparsa
        if (App.Categorie.Count > 0)
        {
            CaricaCategorie();
        }
        if (App.UserDetails.TipoUtente == TipoUtente.Admin)
        {
            AddAdminDashboardPage();
        }

        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
    private static void AddHomePage()
    {
        var flyoutItem = new FlyoutItem
        {
            Title = "Home",
            Route = nameof(HomePage),
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
            Icon = new FontImageSource
            {
                FontFamily = "FaSolid",
                Glyph = FaSolidIcons.House,
                Color = Colors.White
            },
            Items =
            {
                new ShellContent
                {
                    Title = "Home",
                    ContentTemplate = new DataTemplate(typeof(HomePage)),
                    // Route = $"//{nameof(HomePage)}?Categoria={Guid.Empty.ToString()}"
                }
            }
        };
        if (!AppShell.Current.Items.Contains(flyoutItem))
        {
            AppShell.Current.Items.Add(flyoutItem);
        }
    }
    private static void AddAdminDashboardPage()
    {
        var flyoutItem = new FlyoutItem
        {
            Title = "Admin dashboard",
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
            Icon = new FontImageSource
            {
                FontFamily = "FaSolid",
                Glyph = FaSolidIcons.UserGear,
                Color = Colors.White
            },
            Items =
            {
                new ShellContent
                {
                    Title = "Admin dashboard",
                    ContentTemplate = new DataTemplate(typeof(AdminPage)),
                }
            }
        };
        if (!AppShell.Current.Items.Contains(flyoutItem))
        {
            AppShell.Current.Items.Add(flyoutItem);
        }
    }

    private static void CaricaCategorie()
    {
        var flyoutItem = new FlyoutItem()
        {
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems
        };
        foreach (var categoria in App.Categorie)
        {
            var color = Color.FromArgb(categoria.Color);
            if (color.Equals(Colors.Black))
                color = Colors.White;
            var categoriaId = categoria.Id.ToString();
            var menuItem = new MenuItem
            {
                Text = categoria.Descrizione,
                IconImageSource = new FontImageSource
                {
                    FontFamily = "FaSolid",
                    Color = color,
                    Glyph = FaSolidIcons.Circle
                },
                Command = new AsyncRelayCommand(async () =>
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}?Categoria={categoriaId}")),
            };
            // flyoutItem.Items.Add(menuItem);
            AppShell.Current.Items.Add(menuItem);
        }

        var addCategoriaItem = new ShellContent
        {
            Title = "Crea nuova categoria",
            ContentTemplate = new DataTemplate(typeof(AddCategoryPage)),
            Icon = new FontImageSource
            {
                FontFamily = "FaSolid",
                Color = Colors.White,
                Glyph = FaSolidIcons.Plus
            }
        };
        flyoutItem.Items.Add(addCategoriaItem);
        AppShell.Current.Items.Add(flyoutItem);
    }
}