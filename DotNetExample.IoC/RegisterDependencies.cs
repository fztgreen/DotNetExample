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

        services.AddScoped<IGetCar, GetCar>();
        services.AddScoped<IGetCars, GetCars>();
        services.AddScoped<IAddCar, AddCar>();
        services.AddScoped<IUpdateCar, UpdateCar>();
        services.AddScoped<IDeleteCar, DeleteCar>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddSingleton<IInMemoryDatabaseWrapper, InMemoryDatabaseWrapper>();
    }
}
