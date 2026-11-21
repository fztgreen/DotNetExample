namespace DotNetExample.Domain.DataAccess.Foo;

public interface IFooRepository
{
    Task<string> GetFooAsync();
    Task SetFooAsync(string value);
}
