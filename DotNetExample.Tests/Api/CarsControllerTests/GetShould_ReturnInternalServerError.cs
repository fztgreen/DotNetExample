using System.Net;

using DotNetExample.Domain.Entities;

using FluentAssertions;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public partial class GetShould
{
    [TestMethod]
    public async Task ReturnInternalServerError_WhenDatabaseFails()
    {
        // Arrange
        var id = RandomValue.Guid();
        InMemoryDatabase.Find<Car>(CarsCollection, id).Throws(new InvalidOperationException(RandomValue.String()));

        // Act
        var response = await Client.GetAsync($"{CarsUri}/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
