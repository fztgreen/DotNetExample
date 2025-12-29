using DotNetExample.Domain.DataAccess;

namespace DotNetExample.Domain.Queries;

public class GetFoo : IGetFoo
{
    private readonly IFooRepository _fooRepository;

    public GetFoo(IFooRepository fooRepository)
    {
        _fooRepository = fooRepository;
    }

    public Task<string> ExecuteAsync()
    {
        return _fooRepository.GetFooAsync();
    }
}