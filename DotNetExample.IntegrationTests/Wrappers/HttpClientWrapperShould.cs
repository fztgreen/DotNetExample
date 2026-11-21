using DotNetExample.DataAccess.Wrappers;

using Microsoft.Extensions.DependencyInjection;

namespace DotNetExample.IntegrationTests.Wrappers;

[TestClass]
public class HttpClientWrapperShould : HttpClientWrapperContract
{
    private ServiceProvider _services;

    [TestCleanup]
    public void DisposeServices()
    {
        _services.Dispose();
    }

    protected override IHttpClientWrapper CreateWrapper()
    {
        _services = new ServiceCollection().AddHttpClient().BuildServiceProvider();
        return new HttpClientWrapper(_services.GetRequiredService<IHttpClientFactory>());
    }
}
