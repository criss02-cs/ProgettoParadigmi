namespace ProgettoParadigmi.Mobile.Utils;

public class HttpClientFactory
{
    public static HttpClient Create()
    {
        HttpClient client;
#if DEBUG
        client = new HttpClient(HttpsClientHandlerService.CreatePlatformMessageHandler());
#else
        client = new HttpClient();
#endif
        return client;
    }
}