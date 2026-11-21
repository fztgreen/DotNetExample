using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Domain.Queries.Cars;

public interface IGetCar
{
    Task<Car?> ExecuteAsync(Guid id);
}
