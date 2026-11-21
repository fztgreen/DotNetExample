namespace DotNetExample.Domain.Exceptions.Cars;

public class CarNotFoundException : Exception
{
    public CarNotFoundException(Guid id)
        : base($"Car {id} was not found.")
    {
    }
}
