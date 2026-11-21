using System.Net;
using System.Net.Http.Json;

using DotNetExample.Domain.Entities.Cars;

using FluentAssertions;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

[TestClass]
public class GetShould : CarsControllerTestBase
{
    [TestMethod]
    public async Task ReturnOk_WhenCarExists()
    {
        // Arrange
        var car = RandomCar();
        DocumentDatabase.Find<Car>(CarsCollection, car.Id).Returns(car);

        // Act
        var response = await Client.GetAsync($"{CarsUri}/{car.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<Car>(JsonOptions);
        body.Should().BeEquivalentTo(car);
    }

    [TestMethod]
    public async Task ReturnNotFound_WhenCarDoesNotExist()
    {
        // Arrange
        var unknownId = RandomValue.Guid();

        // Act
        var response = await Client.GetAsync($"{CarsUri}/{unknownId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task ReturnInternalServerError_WhenDatabaseFails()
    {
        // Arrange
        var id = RandomValue.Guid();
        DocumentDatabase.Find<Car>(CarsCollection, id).Throws(new InvalidOperationException(RandomValue.String()));

        // Act
        var response = await Client.GetAsync($"{CarsUri}/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
