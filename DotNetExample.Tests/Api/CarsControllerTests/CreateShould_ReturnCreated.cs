using System.Net;
using System.Net.Http.Json;

using DotNetExample.Domain.Entities;

using FluentAssertions;

using NSubstitute;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public partial class CreateShould
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
        var createdCar = await response.Content.ReadFromJsonAsync<Car>(JsonOptions);
        createdCar.Should().BeEquivalentTo(request);
        createdCar!.Id.Should().NotBeEmpty();
        response.Headers.Location!.AbsolutePath.Should().BeEquivalentTo($"/{CarsUri}/{createdCar.Id}");
        InMemoryDatabase.Received(1).Insert(CarsCollection, createdCar.Id, createdCar);
    }
}
