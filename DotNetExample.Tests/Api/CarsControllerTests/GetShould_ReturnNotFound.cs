using System.Net;

using FluentAssertions;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public partial class GetShould
{
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
}
