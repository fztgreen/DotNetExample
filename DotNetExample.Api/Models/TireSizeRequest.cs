using System.ComponentModel.DataAnnotations;

using DotNetExample.Domain.Entities;

namespace DotNetExample.Api.Models;

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
