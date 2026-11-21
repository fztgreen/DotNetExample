namespace DotNetExample.Domain.Queries.Foo;

public interface IGetFoo
{
    Task<string> ExecuteAsync();
}
