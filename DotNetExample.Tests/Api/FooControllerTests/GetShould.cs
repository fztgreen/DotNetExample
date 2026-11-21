using Microsoft.AspNetCore.Mvc;

using NSubstitute;

using RandomTestValues;

namespace DotNetExample.Tests.Api.FooControllerTests;

[TestClass]
public class GetShould : FooControllerTestBase
{
    [TestMethod]
    public async Task ReturnOk()
    {
        // Arrange
        var expected = "Hello from FooController!";
        GetFoo.CurrentAsync().Returns(expected);

        // Act
        var result = await Sut.Get();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.AreEqual(expected, okResult.Value);
    }

    [TestMethod]
    public async Task NotWriteFoo()
    {
        // Arrange
        GetFoo.CurrentAsync().Returns(RandomValue.String());

        // Act
        await Sut.Get();

        // Assert
        await SetFoo.DidNotReceiveWithAnyArgs().ToAsync(default!);
    }
}
