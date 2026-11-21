using System.Net;
using System.Net.Http.Json;

using DotNetExample.Api.Models.Cars;
using DotNetExample.Domain.Entities.Cars;

using FluentAssertions;

using NSubstitute;

namespace DotNetExample.Tests.Api.CarsControllerTests;

[TestClass]
public class GetAllShould : CarsControllerTestBase
{
    [TestMethod]
    public async Task ReturnOk_WithAllCars()
    {
        // Arrange
        Car[] cars = [RandomCar(), RandomCar(), RandomCar()];
        DocumentDatabase.FindAll<Car>(CarsCollection).Returns(cars);

        // Act
        var response = await Client.GetAsync(CarsUri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<CarResponse[]>(JsonOptions);
        body.Should().BeEquivalentTo(cars);
    }
}
