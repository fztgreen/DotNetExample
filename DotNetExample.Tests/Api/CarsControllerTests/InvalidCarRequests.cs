using DotNetExample.Api.Models.Cars;
using DotNetExample.Domain.Entities.Cars;

using RandomTestValues;

namespace DotNetExample.Tests.Api.CarsControllerTests;

public static class InvalidCarRequests
{
    public static IEnumerable<(string InvalidField, Func<CarRequest, CarRequest> Invalidate)> All =>
    [
        ("Make empty", WithEmptyMake),
        ("Make too long", WithTooLongMake),
        ("Model empty", WithEmptyModel),
        ("Model too long", WithTooLongModel),
        ("Length too short", WithTooShortLength),
        ("TireSize missing", WithoutTireSize),
        ("Tire width too narrow", WithTooNarrowTire),
        ("Aspect ratio too low", WithTooLowAspectRatio),
        ("Wheel diameter too small", WithTooSmallWheel),
        ("Engine missing", WithoutEngine),
        ("Displacement zero", WithZeroDisplacement),
        ("Horsepower zero", WithZeroHorsepower),
        ("Torque zero", WithZeroTorque),
        ("Too many cylinders", WithTooManyCylinders),
        ("Undefined shape", WithUndefinedEngineShape)
    ];

    private static CarRequest WithEmptyMake(CarRequest request)
    {
        return request with { Make = string.Empty };
    }

    private static CarRequest WithTooLongMake(CarRequest request)
    {
        return request with { Make = RandomValue.String(51) };
    }

    private static CarRequest WithEmptyModel(CarRequest request)
    {
        return request with { Model = string.Empty };
    }

    private static CarRequest WithTooLongModel(CarRequest request)
    {
        return request with { Model = RandomValue.String(51) };
    }

    private static CarRequest WithTooShortLength(CarRequest request)
    {
        return request with { LengthInMillimeters = 999 };
    }

    private static CarRequest WithoutTireSize(CarRequest request)
    {
        return request with { TireSize = null! };
    }

    private static CarRequest WithTooNarrowTire(CarRequest request)
    {
        return request with { TireSize = request.TireSize with { WidthInMillimeters = 99 } };
    }

    private static CarRequest WithTooLowAspectRatio(CarRequest request)
    {
        return request with { TireSize = request.TireSize with { AspectRatio = 19 } };
    }

    private static CarRequest WithTooSmallWheel(CarRequest request)
    {
        return request with { TireSize = request.TireSize with { WheelDiameterInInches = 9 } };
    }

    private static CarRequest WithoutEngine(CarRequest request)
    {
        return request with { Engine = null! };
    }

    private static CarRequest WithZeroDisplacement(CarRequest request)
    {
        return request with { Engine = request.Engine with { DisplacementInLiters = 0 } };
    }

    private static CarRequest WithZeroHorsepower(CarRequest request)
    {
        return request with { Engine = request.Engine with { Horsepower = 0 } };
    }

    private static CarRequest WithZeroTorque(CarRequest request)
    {
        return request with { Engine = request.Engine with { TorqueInPoundFeet = 0 } };
    }

    private static CarRequest WithTooManyCylinders(CarRequest request)
    {
        return request with { Engine = request.Engine with { Cylinders = 17 } };
    }

    private static CarRequest WithUndefinedEngineShape(CarRequest request)
    {
        return request with { Engine = request.Engine with { Shape = (EngineShape)99 } };
    }
}
