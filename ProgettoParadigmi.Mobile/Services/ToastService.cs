using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ProgettoParadigmi.Mobile.Services;

public static class ToastService
{
    public static Task ShowToast(string text)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var duration = ToastDuration.Short;
        var fontSize = 14;
        var toast = Toast.Make(text, duration, fontSize);
        return toast.Show(cancellationTokenSource.Token);
    }
}