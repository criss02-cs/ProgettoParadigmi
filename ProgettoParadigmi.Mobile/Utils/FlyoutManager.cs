using System.Diagnostics;
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
        RemovePages();
        var newCategoria =
            AppShell.Current.Items.FirstOrDefault(x => x.Items.Any(y => y.Title == "Crea nuova categoria"));
        if (newCategoria is not null) AppShell.Current.Items.Remove(newCategoria);

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
        var flyoutItem = new FlyoutItem()
        {
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems
        };
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

        if (App.UserDetails.TipoUtente == TipoUtente.Admin)
        {
            AddAdminDashboardPage();
        }

        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }

    private static void RemovePages()
    {
        var adminPage = AppShell.Current.Items.FirstOrDefault(f => f.Title == "Admin dashboard");
        if (adminPage != null) AppShell.Current.Items.Remove(adminPage);
        var homePage = AppShell.Current.Items.FirstOrDefault(f => f.Route == nameof(HomePage));
        if (homePage != null) AppShell.Current.Items.Remove(homePage);
        // Devo fare così perché la classe MenuShellItem è internal e non posso accedervi
        var menuItems = AppShell.Current.Items.Where(x => x.GetType().Name == "MenuShellItem").ToList();
        foreach (var item in menuItems)
        {
            Debug.WriteLine(item);
            AppShell.Current.Items.Remove(item);
        }
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
        foreach (var categoria in App.Categorie)
        {
            var color = Color.FromArgb(categoria.Color);
            if (color.Equals(Colors.Black))
                color = Colors.White;
            var categoriaId = categoria.Id.ToString();
            var menuItem = new MenuFlyoutItem()
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
            AppShell.Current.Items.Add(menuItem);
        }
    }
}