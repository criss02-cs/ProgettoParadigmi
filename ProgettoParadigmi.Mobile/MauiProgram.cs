using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using ProgettoParadigmi.Mobile.Services;
using ProgettoParadigmi.Mobile.Services.Appuntamenti;
using ProgettoParadigmi.Mobile.Services.Categorie;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;
using ProgettoParadigmi.Mobile.ViewModels.Startup;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Mobile.Views.Startup;

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
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-brands-400.ttf", "FaBrands");
                fonts.AddFont("fa-solid-900.ttf", "FaSolid");
                fonts.AddFont("TT-Commons-Bold.otf", "TTCommonsBold");
            });

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

        return builder;
    }

    private static MauiAppBuilder UseViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<LoadingPageViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<HomePageViewModel>();
        builder.Services.AddSingleton<AddEventPageViewModel>();
        return builder;
    }

    private static MauiAppBuilder UseViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<AddEventPage>();
        return builder;
    }
}