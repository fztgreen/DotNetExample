using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Entities;

namespace DotNetExample.Domain.Commands;

public class AddCar : IAddCar
{
    private readonly ICarRepository _carRepository;

    public AddCar(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public Task ExecuteAsync(Car car)
    {
        return _carRepository.AddCarAsync(car);
    }
}
