using DotNetExample.Domain.DataAccess.Foo;

namespace DotNetExample.Domain.Commands.Foo;

public class SetFoo : ISetFoo
{
    private readonly IFooRepository _fooRepository;

    public SetFoo(IFooRepository fooRepository)
    {
        _fooRepository = fooRepository;
    }

    public Task ExecuteAsync(string value)
    {
        return _fooRepository.SetFooAsync(value);
    }
}
