using DotNetExample.DataAccess.Wrappers;
using DotNetExample.Domain.DataAccess;

namespace DotNetExample.DataAccess;

public class FooRepository : IFooRepository
{
    private readonly IHttpClientWrapper _httpClientWrapper;

    public FooRepository(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }

    public Task<string> GetFooAsync()
    {
        return _httpClientWrapper.GetAsync("https://foo.com");
    }

    public Task SetFooAsync(string value)
    {
        // For the sake of this example, we'll just return a completed task.
        return Task.CompletedTask;
    }
}
