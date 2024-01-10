using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Mopups.Hosting;
using ProgettoParadigmi.Mobile.Services;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
using ProgettoParadigmi.Mobile.Services.Categorie;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;
using ProgettoParadigmi.Mobile.ViewModels.Startup;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Mobile.Views.Startup;
using UraniumUI;

namespace ProgettoParadigmi.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseServices()
            .UseViews()
            .UseViewModels()
            .ConfigureMopups()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-brands-400.ttf", "FaBrands");
                fonts.AddFont("fa-solid-900.ttf", "FaSolid");
                fonts.AddFont("TT-Commons-Bold.otf", "TTCommonsBold");
            });

        builder.Services.AddMopupsDialogs();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder UseServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ILoginService, LoginService>();
        builder.Services.AddSingleton<IAppuntamentiService, AppuntamentiService>();
        builder.Services.AddSingleton<ICategorieService, CategorieService>();
        builder.Services.AddSingleton<IUserService, UserService>();
        return builder;
    }

    private static MauiAppBuilder UseViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<LoadingPageViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<RegisterPageViewModel>();
        builder.Services.AddSingleton<HomePageViewModel>();
        builder.Services.AddSingleton<AddEventPageViewModel>();
        builder.Services.AddSingleton<AddCategoryPageViewModel>();
        builder.Services.AddSingleton<AdminPageViewModel>();
        builder.Services.AddSingleton<EventDetailsViewModel>();
        builder.Services.AddSingleton<NotificationPageViewModel>();
        return builder;
    }

    private static MauiAppBuilder UseViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<RegisterPage>();
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<AddEventPage>();
        builder.Services.AddSingleton<AddCategoryPage>();
        builder.Services.AddSingleton<AdminPage>();
        builder.Services.AddSingleton<EventDetailsPage>();
        builder.Services.AddSingleton<NotificationPage>();
        return builder;
    }
}