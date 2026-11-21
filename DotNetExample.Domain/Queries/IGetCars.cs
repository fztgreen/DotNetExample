using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Queries;

public interface IGetCars
{
    Task<IReadOnlyCollection<Car>> ExecuteAsync();
}
