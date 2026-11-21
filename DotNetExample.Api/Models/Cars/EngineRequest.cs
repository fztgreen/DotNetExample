using System.ComponentModel.DataAnnotations;

using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Api.Models.Cars;

public record EngineRequest
{
    [Range(0.1, 20.0)]
    public double DisplacementInLiters { get; init; }

    [Range(1, 2000)]
    public int Horsepower { get; init; }

    [Range(1, 2000)]
    public int TorqueInPoundFeet { get; init; }

    [Range(0, 16)]
    public int Cylinders { get; init; }

    [EnumDataType(typeof(EngineShape))]
    public EngineShape Shape { get; init; }

    public Engine ToEngine()
    {
        return new Engine(DisplacementInLiters, Horsepower, TorqueInPoundFeet, Cylinders, Shape);
    }
}
