using DotNetExample.Domain.DataAccess.Cars;
using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Domain.Commands.Cars;

public class AddCar : IAddCar
{
    private readonly ICarRepository _carRepository;

    public AddCar(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public Task WithAsync(Car car)
    {
        return _carRepository.AddCarAsync(car);
    }
}
