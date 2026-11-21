using DotNetExample.Domain.DataAccess.Foo;

namespace DotNetExample.Domain.Queries.Foo;

public class GetFoo : IGetFoo
{
    private readonly IFooRepository _fooRepository;

    public GetFoo(IFooRepository fooRepository)
    {
        _fooRepository = fooRepository;
    }

    public Task<string> CurrentAsync()
    {
        return _fooRepository.GetFooAsync();
    }
}
