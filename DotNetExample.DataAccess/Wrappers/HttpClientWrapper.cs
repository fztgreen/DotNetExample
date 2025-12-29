namespace DotNetExample.DataAccess.Wrappers;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;

    public HttpClientWrapper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<string> GetAsync(string requestUri)
    {
        return _httpClient.GetStringAsync(requestUri);
    }
}
