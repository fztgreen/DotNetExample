using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.DataAccess;

public interface ICarRepository
{
    Task<Car?> GetCarAsync(Guid id);

    Task<IReadOnlyCollection<Car>> GetCarsAsync();

    Task AddCarAsync(Car car);

    Task<bool> UpdateCarAsync(Car car);

    Task<bool> DeleteCarAsync(Guid id);
}
