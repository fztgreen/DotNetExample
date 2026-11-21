using DotNetExample.DataAccess.Cars;
using DotNetExample.DataAccess.Foo;
using DotNetExample.DataAccess.Wrappers;
using DotNetExample.Domain.Commands.Cars;
using DotNetExample.Domain.Commands.Foo;
using DotNetExample.Domain.DataAccess.Cars;
using DotNetExample.Domain.DataAccess.Foo;
using DotNetExample.Domain.Queries.Cars;
using DotNetExample.Domain.Queries.Foo;

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
