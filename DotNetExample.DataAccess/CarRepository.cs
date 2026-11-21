using DotNetExample.DataAccess.Wrappers;
using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Entities;

namespace DotNetExample.DataAccess;

public class CarRepository : ICarRepository
{
    private const string CarsCollection = "cars";

    private readonly IInMemoryDatabaseWrapper _inMemoryDatabase;

    public CarRepository(IInMemoryDatabaseWrapper inMemoryDatabase)
    {
        _inMemoryDatabase = inMemoryDatabase;
    }

    public Task<Car?> GetCarAsync(Guid id)
    {
        return Task.FromResult(_inMemoryDatabase.Find<Car>(CarsCollection, id));
    }

    public Task<IReadOnlyCollection<Car>> GetCarsAsync()
    {
        return Task.FromResult(_inMemoryDatabase.FindAll<Car>(CarsCollection));
    }

    public Task AddCarAsync(Car car)
    {
        _inMemoryDatabase.Insert(CarsCollection, car.Id, car);
        return Task.CompletedTask;
    }

    public Task<bool> UpdateCarAsync(Car car)
    {
        return Task.FromResult(_inMemoryDatabase.Replace(CarsCollection, car.Id, car));
    }

    public Task<bool> DeleteCarAsync(Guid id)
    {
        return Task.FromResult(_inMemoryDatabase.Delete(CarsCollection, id));
    }
}
