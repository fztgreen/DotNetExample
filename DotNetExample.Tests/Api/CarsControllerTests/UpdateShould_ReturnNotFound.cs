using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public partial class UpdateShould
{
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
}
