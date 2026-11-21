using System.Net;
using System.Net.Http.Json;

using DotNetExample.Domain.Entities;

using FluentAssertions;

using NSubstitute;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public partial class GetAllShould
{
    [TestMethod]
    public async Task ReturnOk_WithAllCars()
    {
        // Arrange
        Car[] cars = [RandomCar(), RandomCar(), RandomCar()];
        InMemoryDatabase.FindAll<Car>(CarsCollection).Returns(cars);

        // Act
        var response = await Client.GetAsync(CarsUri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<Car[]>(JsonOptions);
        body.Should().BeEquivalentTo(cars);
    }
}
