using DotNetExample.Domain.DataAccess;
using DotNetExample.Domain.Entities;
using DotNetExample.Domain.Exceptions;

namespace DotNetExample.Domain.Commands;

public class UpdateCar : IUpdateCar
{
    private readonly ICarRepository _carRepository;

    public UpdateCar(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task ExecuteAsync(Car car)
    {
        var carWasUpdated = await _carRepository.UpdateCarAsync(car);
        if (!carWasUpdated)
        {
            throw new CarNotFoundException(car.Id);
        }
    }
}
