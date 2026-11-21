namespace DotNetExample.Domain.Commands.Cars;

public interface IDeleteCar
{
    Task ExecuteAsync(Guid id);
}
