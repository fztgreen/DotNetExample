using DotNetExample.Domain.DataAccess.Cars;
using DotNetExample.Domain.Exceptions.Cars;

namespace DotNetExample.Domain.Commands.Cars;

public class DeleteCar : IDeleteCar
{
    private readonly ICarRepository _carRepository;

    public DeleteCar(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task ByAsync(Guid id)
    {
        var carWasDeleted = await _carRepository.DeleteCarAsync(id);
        if (!carWasDeleted)
        {
            throw new CarNotFoundException(id);
        }
    }
}
