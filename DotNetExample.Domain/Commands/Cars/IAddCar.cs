using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Domain.Commands.Cars;

public interface IAddCar
{
    Task WithAsync(Car car);
}
