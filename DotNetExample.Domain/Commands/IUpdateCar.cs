using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Commands;

public interface IUpdateCar
{
    Task ExecuteAsync(Car car);
}
