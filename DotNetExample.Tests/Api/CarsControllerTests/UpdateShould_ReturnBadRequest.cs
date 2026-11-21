using System.Net;
using System.Net.Http.Json;

using DotNetExample.Api.Models;
using DotNetExample.Domain.Entities;

using FluentAssertions;

using NSubstitute;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public partial class UpdateShould
{
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
        InMemoryDatabase.DidNotReceiveWithAnyArgs().Replace<Car>(default!, default, default!);
    }
}
