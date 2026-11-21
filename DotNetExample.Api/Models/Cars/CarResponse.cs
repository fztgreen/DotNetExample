using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Api.Models.Cars;

public record CarResponse(
    Guid Id,
    string Make,
    string Model,
    int LengthInMillimeters,
    TireSizeResponse TireSize,
    EngineResponse Engine)
{
    public static CarResponse From(Car car)
    {
        return new CarResponse(
            car.Id,
            car.Make,
            car.Model,
            car.LengthInMillimeters,
            TireSizeResponse.From(car.TireSize),
            EngineResponse.From(car.Engine));
    }
}
