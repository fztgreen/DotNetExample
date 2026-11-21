using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

using RandomTestValues;

namespace DotNetExample.Tests.Api.FooControllerTests;

[TestClass]
public class PutShould : FooControllerTestBase
{
    [TestMethod]
    public async Task ReturnNoContent_AndWriteFoo()
    {
        // Arrange
        var value = RandomValue.String();

        // Act
        var result = await Sut.Put(value);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        await SetFoo.Received(1).ToAsync(value);
    }
}
