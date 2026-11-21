using DotNetExample.Api.Models.Cars;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public static class InvalidCarRequests
{
    public static IEnumerable<(string InvalidField, Func<CarRequest, CarRequest> Invalidate)> All =>
    [
        ("Make empty", request => request with { Make = string.Empty }),
        ("Make too long", request => request with { Make = RandomValue.String(51) }),
        ("Model empty", request => request with { Model = string.Empty }),
        ("Model too long", request => request with { Model = RandomValue.String(51) }),
        ("Length too short", request => request with { LengthInMillimeters = 999 }),
        ("TireSize missing", request => request with { TireSize = null! }),
        ("Tire width too narrow", request => request with { TireSize = request.TireSize with { WidthInMillimeters = 99 } }),
        ("Aspect ratio too low", request => request with { TireSize = request.TireSize with { AspectRatio = 19 } }),
        ("Wheel diameter too small", request => request with { TireSize = request.TireSize with { WheelDiameterInInches = 9 } }),
        ("Engine missing", request => request with { Engine = null! }),
        ("Displacement zero", request => request with { Engine = request.Engine with { DisplacementInLiters = 0 } }),
        ("Horsepower zero", request => request with { Engine = request.Engine with { Horsepower = 0 } }),
        ("Torque zero", request => request with { Engine = request.Engine with { TorqueInPoundFeet = 0 } }),
        ("Too many cylinders", request => request with { Engine = request.Engine with { Cylinders = 17 } }),
        ("Undefined shape", request => request with { Engine = request.Engine with { Shape = (EngineShape)99 } })
    ];
}
