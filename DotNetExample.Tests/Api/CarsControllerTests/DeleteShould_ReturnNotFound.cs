using System.Net;

using FluentAssertions;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public partial class DeleteShould
{
    [TestMethod]
    public async Task ReturnNotFound_WhenCarDoesNotExist()
    {
        // Arrange
        var unknownId = RandomValue.Guid();

        // Act
        var response = await Client.DeleteAsync($"{CarsUri}/{unknownId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
