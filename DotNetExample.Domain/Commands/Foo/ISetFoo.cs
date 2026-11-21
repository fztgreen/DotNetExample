namespace DotNetExample.Domain.Commands.Foo;

public interface ISetFoo
{
    Task ExecuteAsync(string value);
}
