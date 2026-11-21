using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Api.Models.Cars;

public record TireSizeResponse(int WidthInMillimeters, int AspectRatio, int WheelDiameterInInches)
{
    public static TireSizeResponse From(TireSize tireSize)
    {
        return new TireSizeResponse(tireSize.WidthInMillimeters, tireSize.AspectRatio, tireSize.WheelDiameterInInches);
    }
}
