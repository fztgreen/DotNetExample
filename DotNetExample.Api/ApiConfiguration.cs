
using Microsoft.Extensions.DependencyInjection;

namespace DotNetExample.Api;

public static class ApiConfiguration
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();
        return services;
    }
}
