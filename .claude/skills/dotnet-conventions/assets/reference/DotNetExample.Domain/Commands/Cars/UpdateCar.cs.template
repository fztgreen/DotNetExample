using DotNetExample.Domain.DataAccess.Cars;
using DotNetExample.Domain.Entities.Cars;
using DotNetExample.Domain.Exceptions.Cars;

namespace DotNetExample.Domain.Commands.Cars;

public class UpdateCar : IUpdateCar
{
    private readonly ICarRepository _carRepository;

    public UpdateCar(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task WithAsync(Car car)
    {
        var carWasUpdated = await _carRepository.UpdateCarAsync(car);
        if (!carWasUpdated)
        {
            throw new CarNotFoundException(car.Id);
        }
    }
}
