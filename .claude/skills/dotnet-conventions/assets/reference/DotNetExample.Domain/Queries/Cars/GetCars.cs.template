using DotNetExample.Domain.DataAccess.Cars;
using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Domain.Queries.Cars;

public class GetCars : IGetCars
{
    private readonly ICarRepository _carRepository;

    public GetCars(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public Task<IReadOnlyCollection<Car>> AllAsync()
    {
        return _carRepository.GetCarsAsync();
    }
}
