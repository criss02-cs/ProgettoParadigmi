using Microsoft.Maui.Controls;
using ProgettoParadigmi.Mobile.Handlers;
using ProgettoParadigmi.Models.Dto;
#if __ANDROID__
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

namespace ProgettoParadigmi.Mobile;

public partial class App : Application
{
    public static AuthDto UserDetails;
    public static string Token;

    public App()
    {
        InitializeComponent();
        if (Application.Current != null) Application.Current.UserAppTheme = AppTheme.Dark;
        // Border less entry
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            }
        });
        MainPage = new AppShell();
    }
}