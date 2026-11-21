using System.Net;
using System.Net.Http.Json;

using DotNetExample.Api.Models.Cars;
using DotNetExample.Domain.Entities.Cars;

using FluentAssertions;

using NSubstitute;

namespace DotNetExample.Tests.Api.CarsControllerTests;

[TestClass]
public class CreateShould : CarsControllerTestBase
{
    [TestMethod]
    public async Task ReturnCreated_AndStoreCar()
    {
        // Arrange
        var request = RandomCarRequest();

        // Act
        var response = await Client.PostAsJsonAsync(CarsUri, request, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdCar = await response.Content.ReadFromJsonAsync<CarResponse>(JsonOptions);
        createdCar.Should().BeEquivalentTo(request);
        createdCar!.Id.Should().NotBeEmpty();
        response.Headers.Location!.AbsolutePath.Should().BeEquivalentTo($"/{CarsUri}/{createdCar.Id}");
        DocumentDatabase.Received(1).Insert(CarsCollection, createdCar.Id, ExpectedCar(request, createdCar.Id));
    }

    [TestMethod]
    [DynamicData(nameof(InvalidCarRequests.All), typeof(InvalidCarRequests))]
    public async Task ReturnBadRequest_WhenRequestIsInvalid(string invalidField, Func<CarRequest, CarRequest> invalidate)
    {
        // Arrange
        var request = invalidate(RandomCarRequest());

        // Act
        var response = await Client.PostAsJsonAsync(CarsUri, request, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, invalidField);
        DocumentDatabase.DidNotReceiveWithAnyArgs().Insert<Car>(default!, default, default!);
    }
}
