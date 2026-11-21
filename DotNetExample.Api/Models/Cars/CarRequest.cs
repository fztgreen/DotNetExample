using System.ComponentModel.DataAnnotations;

using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Api.Models.Cars;

public record CarRequest
{
    [Required]
    [StringLength(50)]
    public string Make { get; init; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Model { get; init; } = string.Empty;

    [Range(1000, 10000)]
    public int LengthInMillimeters { get; init; }

    public TireSizeRequest TireSize { get; init; } = new();

    public EngineRequest Engine { get; init; } = new();

    public Car ToCar(Guid id)
    {
        return new Car(id, Make, Model, LengthInMillimeters, TireSize.ToTireSize(), Engine.ToEngine());
    }
}
