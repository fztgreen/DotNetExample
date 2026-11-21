using System.Net;
using System.Net.Http.Json;

using DotNetExample.Domain.Entities;

using FluentAssertions;

using NSubstitute;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public partial class UpdateShould
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
        InMemoryDatabase.Replace(CarsCollection, id, updatedCar).Returns(true);

        // Act
        var response = await Client.PutAsJsonAsync($"{CarsUri}/{id}", request, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
