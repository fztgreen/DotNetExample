using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Queries;

public class GetCars : IGetCars
{
    private readonly ICarRepository _carRepository;

    public GetCars(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public Task<IReadOnlyCollection<Car>> ExecuteAsync()
    {
        return _carRepository.GetCarsAsync();
    }
}
