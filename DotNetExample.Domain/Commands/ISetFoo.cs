namespace DotNetExample.Domain.Commands;

public interface ISetFoo
{
    Task ExecuteAsync(string value);
}
