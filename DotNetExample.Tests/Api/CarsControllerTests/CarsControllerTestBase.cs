using System.Text.Json;
using System.Text.Json.Serialization;

using DotNetExample.Api.Models.Cars;
using DotNetExample.DataAccess.Wrappers;
using DotNetExample.Domain.Entities.Cars;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public abstract class CarsControllerTestBase
{
    protected const string CarsUri = "api/cars";
    protected const string CarsCollection = "cars";

    protected static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private WebApplicationFactory<Program> _factory;

    protected IDocumentDatabaseWrapper DocumentDatabase { get; private set; }

    protected HttpClient Client { get; private set; }

    [TestInitialize]
    public void StartTestServer()
    {
        DocumentDatabase = Substitute.For<IDocumentDatabaseWrapper>();
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureTestServices(ReplaceDocumentDatabase));
        Client = _factory.CreateClient();
    }

    [TestCleanup]
    public void StopTestServer()
    {
        Client.Dispose();
        _factory.Dispose();
    }

    protected static Car RandomCar()
    {
        return new Car(
            RandomValue.Guid(),
            RandomValue.String(10),
            RandomValue.String(10),
            RandomValue.Int(6000, 3000),
            new TireSize(RandomValue.Int(335, 155), RandomValue.Int(80, 25), RandomValue.Int(22, 14)),
            new Engine(
                RandomValue.Int(80, 10) / 10.0,
                RandomValue.Int(800, 70),
                RandomValue.Int(700, 70),
                RandomValue.Int(16, 1),
                RandomValue.Enum<EngineShape>()));
    }

    protected static CarRequest RandomCarRequest()
    {
        return new CarRequest
        {
            Make = RandomValue.String(10),
            Model = RandomValue.String(10),
            LengthInMillimeters = RandomValue.Int(6000, 3000),
            TireSize = new TireSizeRequest
            {
                WidthInMillimeters = RandomValue.Int(335, 155),
                AspectRatio = RandomValue.Int(80, 25),
                WheelDiameterInInches = RandomValue.Int(22, 14)
            },
            Engine = new EngineRequest
            {
                DisplacementInLiters = RandomValue.Int(80, 10) / 10.0,
                Horsepower = RandomValue.Int(800, 70),
                TorqueInPoundFeet = RandomValue.Int(700, 70),
                Cylinders = RandomValue.Int(16, 1),
                Shape = RandomValue.Enum<EngineShape>()
            }
        };
    }

    private void ReplaceDocumentDatabase(IServiceCollection services)
    {
        services.AddSingleton(DocumentDatabase);
    }
}
