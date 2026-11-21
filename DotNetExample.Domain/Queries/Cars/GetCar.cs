using DotNetExample.Domain.DataAccess.Cars;
using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Domain.Queries.Cars;

public class GetCar : IGetCar
{
    private readonly ICarRepository _carRepository;

    public GetCar(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public Task<Car?> ExecuteAsync(Guid id)
    {
        return _carRepository.GetCarAsync(id);
    }
}
