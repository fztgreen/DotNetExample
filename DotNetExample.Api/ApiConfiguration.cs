using System.Text.Json.Serialization;

using Microsoft.Extensions.DependencyInjection;

namespace DotNetExample.Api;

public static class ApiConfiguration
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        return services;
    }
}
