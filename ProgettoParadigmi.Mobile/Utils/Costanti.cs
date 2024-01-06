using System.Text.Json;

namespace ProgettoParadigmi.Mobile.Utils;

public static class Costanti
{
    // public static string BaseAddress =
    //     DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7297" : "https://localhost:7297";

    public static string BaseAddress = "https://progettoparadigmi.azurewebsites.net";

    public static JsonSerializerOptions DefaultOptions = new() { PropertyNameCaseInsensitive = true };
}