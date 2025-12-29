using DotNetExample.Domain.DataAccess;

namespace DotNetExample.Domain.Commands;

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
