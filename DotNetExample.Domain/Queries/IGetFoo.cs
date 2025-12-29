namespace DotNetExample.Domain.Queries;

public interface IGetFoo
{
    Task<string> ExecuteAsync();
}