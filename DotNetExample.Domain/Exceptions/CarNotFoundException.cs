namespace DotNetExample.Domain.Exceptions;

public class CarNotFoundException : Exception
{
    public CarNotFoundException(Guid id)
        : base($"Car {id} was not found.")
    {
    }
}
