using DotNetExample.Api.Controllers;
using DotNetExample.Domain.Commands;
using DotNetExample.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DotNetExample.Tests.Api.FooControllerTests;

[TestClass]
public class GetShould
{
    private IGetFoo _getFoo;
    private ISetFoo _setFoo;
    private FooController _sut;

    [TestInitialize]
    public void TestInitialize()
    {
        _getFoo = Substitute.For<IGetFoo>();
        _setFoo = Substitute.For<ISetFoo>();
        _sut = new FooController(_getFoo, _setFoo);
    }

    [TestMethod]
    public async Task ReturnOk()
    {
        // Arrange
        var expected = "Hello from FooController!";
        _getFoo.ExecuteAsync().Returns(expected);

        // Act
        var result = await _sut.Get();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.AreEqual(expected, okResult.Value);
        await _setFoo.Received(1).ExecuteAsync(expected);
    }
}
