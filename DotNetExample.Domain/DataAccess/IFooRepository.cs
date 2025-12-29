namespace DotNetExample.Domain.DataAccess;

public interface IFooRepository
{
    Task<string> GetFooAsync();
    Task SetFooAsync(string value);
}