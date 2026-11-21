namespace DotNetExample.Domain.Commands.Foo;

public interface ISetFoo
{
    Task ToAsync(string value);
}
