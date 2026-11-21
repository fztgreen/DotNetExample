using System.Net;
using System.Net.Http.Json;

using DotNetExample.Api.Models.Cars;
using DotNetExample.Domain.Entities.Cars;

using FluentAssertions;

using NSubstitute;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

[TestClass]
public class UpdateShould : CarsControllerTestBase
{
    [TestMethod]
    public async Task ReturnNoContent_WhenCarExists()
    {
        // Arrange
        var id = RandomValue.Guid();
        var request = RandomCarRequest();
        var updatedCar = new Car(
            id,
            request.Make,
            request.Model,
            request.LengthInMillimeters,
            new TireSize(request.TireSize.WidthInMillimeters, request.TireSize.AspectRatio, request.TireSize.WheelDiameterInInches),
            new Engine(
                request.Engine.DisplacementInLiters,
                request.Engine.Horsepower,
                request.Engine.TorqueInPoundFeet,
                request.Engine.Cylinders,
                request.Engine.Shape));
        DocumentDatabase.Replace(CarsCollection, id, updatedCar).Returns(true);

        // Act
        var response = await Client.PutAsJsonAsync($"{CarsUri}/{id}", request, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [TestMethod]
    public async Task ReturnNotFound_WhenCarDoesNotExist()
    {
        // Arrange
        var unknownId = RandomValue.Guid();

        // Act
        var response = await Client.PutAsJsonAsync($"{CarsUri}/{unknownId}", RandomCarRequest(), JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    [DynamicData(nameof(InvalidCarRequests))]
    public async Task ReturnBadRequest_WhenRequestIsInvalid(string invalidField, Func<CarRequest, CarRequest> invalidate)
    {
        // Arrange
        var request = invalidate(RandomCarRequest());

        // Act
        var response = await Client.PutAsJsonAsync($"{CarsUri}/{RandomValue.Guid()}", request, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, invalidField);
        DocumentDatabase.DidNotReceiveWithAnyArgs().Replace<Car>(default!, default, default!);
    }
}
