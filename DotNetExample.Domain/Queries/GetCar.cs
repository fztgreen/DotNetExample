using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Queries;

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
