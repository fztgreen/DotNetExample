using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Api.Models.Cars;

public record EngineResponse(
    double DisplacementInLiters,
    int Horsepower,
    int TorqueInPoundFeet,
    int Cylinders,
    EngineShape Shape)
{
    public static EngineResponse From(Engine engine)
    {
        return new EngineResponse(
            engine.DisplacementInLiters,
            engine.Horsepower,
            engine.TorqueInPoundFeet,
            engine.Cylinders,
            engine.Shape.ToApi());
    }
}
