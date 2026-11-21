using DomainEngineShape = DotNetExample.Domain.Entities.Cars.EngineShape;

namespace DotNetExample.Api.Models.Cars;

public static class EngineShapeMapping
{
    public static DomainEngineShape ToDomain(this EngineShape shape)
    {
        return shape switch
        {
            EngineShape.Inline => DomainEngineShape.Inline,
            EngineShape.V => DomainEngineShape.V,
            EngineShape.Flat => DomainEngineShape.Flat,
            EngineShape.W => DomainEngineShape.W,
            EngineShape.Rotary => DomainEngineShape.Rotary,
            _ => throw new ArgumentOutOfRangeException(nameof(shape), shape, "Unknown engine shape.")
        };
    }

    public static EngineShape ToApi(this DomainEngineShape shape)
    {
        return shape switch
        {
            DomainEngineShape.Inline => EngineShape.Inline,
            DomainEngineShape.V => EngineShape.V,
            DomainEngineShape.Flat => EngineShape.Flat,
            DomainEngineShape.W => EngineShape.W,
            DomainEngineShape.Rotary => EngineShape.Rotary,
            _ => throw new ArgumentOutOfRangeException(nameof(shape), shape, "Unknown engine shape.")
        };
    }
}
