namespace DotNetExample.Domain.Entities;

public record Engine(
    double DisplacementInLiters,
    int Horsepower,
    int TorqueInPoundFeet,
    int Cylinders,
    EngineShape Shape);
