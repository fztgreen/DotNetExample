using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Queries;

public interface IGetCar
{
    Task<Car?> ExecuteAsync(Guid id);
}
