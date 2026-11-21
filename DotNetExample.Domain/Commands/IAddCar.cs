using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Commands;

public interface IAddCar
{
    Task ExecuteAsync(Car car);
}
