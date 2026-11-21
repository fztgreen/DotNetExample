using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Domain.DataAccess.Cars;

public interface ICarRepository
{
    Task<Car?> GetCarAsync(Guid id);

    Task<IReadOnlyCollection<Car>> GetCarsAsync();

    Task AddCarAsync(Car car);

    Task<bool> UpdateCarAsync(Car car);

    Task<bool> DeleteCarAsync(Guid id);
}
