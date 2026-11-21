namespace DotNetExample.DataAccess.Wrappers;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;

    public HttpClientWrapper(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public Task<string> GetAsync(string requestUri)
    {
        return _httpClient.GetStringAsync(requestUri);
    }
}
