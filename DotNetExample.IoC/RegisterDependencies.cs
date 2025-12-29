using DotNetExample.DataAccess;
using DotNetExample.DataAccess.Wrappers;
using DotNetExample.Domain.Commands;
using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Queries;

namespace DotNetExample.IoC;

public static class RegisterDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IGetFoo, GetFoo>();
        services.AddScoped<ISetFoo, SetFoo>();
        services.AddScoped<IFooRepository, FooRepository>();
        services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
        services.AddSingleton<HttpClient>();
    }
}
