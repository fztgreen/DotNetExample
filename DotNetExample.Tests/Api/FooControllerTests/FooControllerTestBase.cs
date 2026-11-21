using DotNetExample.Api.Controllers;
using DotNetExample.Domain.Commands.Foo;
using DotNetExample.Domain.Queries.Foo;

using NSubstitute;

namespace DotNetExample.Tests.Api.FooControllerTests;

public abstract class FooControllerTestBase
{
    protected IGetFoo GetFoo { get; private set; }

    protected ISetFoo SetFoo { get; private set; }

    protected FooController Sut { get; private set; }

    [TestInitialize]
    public void CreateController()
    {
        GetFoo = Substitute.For<IGetFoo>();
        SetFoo = Substitute.For<ISetFoo>();
        Sut = new FooController(GetFoo, SetFoo);
    }
}
