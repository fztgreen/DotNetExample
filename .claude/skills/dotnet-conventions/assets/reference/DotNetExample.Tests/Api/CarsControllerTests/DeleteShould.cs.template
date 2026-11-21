using System.Net;

using FluentAssertions;

using NSubstitute;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

[TestClass]
public class DeleteShould : CarsControllerTestBase
{
    [TestMethod]
    public async Task ReturnNoContent_WhenCarExists()
    {
        // Arrange
        var id = RandomValue.Guid();
        DocumentDatabase.Delete(CarsCollection, id).Returns(true);

        // Act
        var response = await Client.DeleteAsync($"{CarsUri}/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

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
