using DotNetExample.DataAccess.Wrappers;
using DotNetExample.Domain.DataAccess.Cars;
using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.DataAccess.Cars;

public class CarRepository : ICarRepository
{
    private const string CarsCollection = "cars";

    private readonly IDocumentDatabaseWrapper _documentDatabase;

    public CarRepository(IDocumentDatabaseWrapper documentDatabase)
    {
        _documentDatabase = documentDatabase;
    }

    public Task<Car?> GetCarAsync(Guid id)
    {
        return Task.FromResult(_documentDatabase.Find<Car>(CarsCollection, id));
    }

    public Task<IReadOnlyCollection<Car>> GetCarsAsync()
    {
        return Task.FromResult(_documentDatabase.FindAll<Car>(CarsCollection));
    }

    public Task AddCarAsync(Car car)
    {
        _documentDatabase.Insert(CarsCollection, car.Id, car);
        return Task.CompletedTask;
    }

    public Task<bool> UpdateCarAsync(Car car)
    {
        return Task.FromResult(_documentDatabase.Replace(CarsCollection, car.Id, car));
    }

    public Task<bool> DeleteCarAsync(Guid id)
    {
        return Task.FromResult(_documentDatabase.Delete(CarsCollection, id));
    }
}
