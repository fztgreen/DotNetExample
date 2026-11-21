using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Domain.Commands.Cars;

public interface IUpdateCar
{
    Task ExecuteAsync(Car car);
}
