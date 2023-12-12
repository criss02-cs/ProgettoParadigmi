using System.Net.Http.Headers;

namespace ProgettoParadigmi.Mobile.Utils;

public static class HttpClientFactory
{
    public static HttpClient Create()
    {
        HttpClient client;
#if DEBUG
        client = new HttpClient(HttpsClientHandlerService.CreatePlatformMessageHandler());
#else
        client = new HttpClient();
#endif
        if (!string.IsNullOrEmpty(App.Token))
        {
            // client.DefaultRequestHeaders.Add("Authorization", App.Token);
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", App.Token);
        }
        return client;
    }
}