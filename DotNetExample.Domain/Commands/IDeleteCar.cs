namespace DotNetExample.Domain.Commands;

public interface IDeleteCar
{
    Task ExecuteAsync(Guid id);
}
