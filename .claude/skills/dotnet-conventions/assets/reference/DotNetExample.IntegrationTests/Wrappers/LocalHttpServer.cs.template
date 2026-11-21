using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotNetExample.IntegrationTests.Wrappers;

public sealed class LocalHttpServer : IAsyncDisposable
{
    public const string FailurePath = "/failure";

    private readonly WebApplication _app;

    private LocalHttpServer(WebApplication app)
    {
        _app = app;
    }

    public string BaseAddress => _app.Urls.First();

    public static async Task<LocalHttpServer> StartAsync()
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.Logging.ClearProviders();
        var app = builder.Build();
        app.Urls.Add("http://127.0.0.1:0");
        app.MapGet(FailurePath, RespondWithServerError);
        app.MapGet("/{**path}", EchoPathAndQuery);
        await app.StartAsync();
        return new LocalHttpServer(app);
    }

    public async ValueTask DisposeAsync()
    {
        await _app.DisposeAsync();
    }

    private static string EchoPathAndQuery(HttpRequest request)
    {
        return $"{request.Path}{request.QueryString}";
    }

    private static IResult RespondWithServerError()
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
}
