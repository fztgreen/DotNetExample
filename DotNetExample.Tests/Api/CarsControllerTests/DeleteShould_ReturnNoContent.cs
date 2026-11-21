using System.Net;

using FluentAssertions;

using NSubstitute;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public partial class DeleteShould
{
    [TestMethod]
    public async Task ReturnNoContent_WhenCarExists()
    {
        // Arrange
        var id = RandomValue.Guid();
        InMemoryDatabase.Delete(CarsCollection, id).Returns(true);

        // Act
        var response = await Client.DeleteAsync($"{CarsUri}/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
