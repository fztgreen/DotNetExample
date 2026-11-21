using System.Net;
using System.Net.Http.Json;

using DotNetExample.Domain.Entities;

using FluentAssertions;

using NSubstitute;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public partial class GetShould
{
    [TestMethod]
    public async Task ReturnOk_WhenCarExists()
    {
        // Arrange
        var car = RandomCar();
        InMemoryDatabase.Find<Car>(CarsCollection, car.Id).Returns(car);

        // Act
        var response = await Client.GetAsync($"{CarsUri}/{car.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<Car>(JsonOptions);
        body.Should().BeEquivalentTo(car);
    }
}
