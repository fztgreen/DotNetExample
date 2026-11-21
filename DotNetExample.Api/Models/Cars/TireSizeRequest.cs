using System.ComponentModel.DataAnnotations;

using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Api.Models.Cars;

public record TireSizeRequest
{
    [Range(100, 400)]
    public int WidthInMillimeters { get; init; }

    [Range(20, 90)]
    public int AspectRatio { get; init; }

    [Range(10, 30)]
    public int WheelDiameterInInches { get; init; }

    public TireSize ToTireSize()
    {
        return new TireSize(WidthInMillimeters, AspectRatio, WheelDiameterInInches);
    }
}
