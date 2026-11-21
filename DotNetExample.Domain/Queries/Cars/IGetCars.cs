using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Domain.Queries.Cars;

public interface IGetCars
{
    Task<IReadOnlyCollection<Car>> ExecuteAsync();
}
