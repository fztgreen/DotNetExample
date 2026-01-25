namespace DotNetExample.Domain.Commands;

public interface IDeleteStudent
{
    Task ExecuteAsync(string id);
}
