namespace DotNetExample.Domain.Entities;

public record Car(
    Guid Id,
    string Make,
    string Model,
    int LengthInMillimeters,
    TireSize TireSize,
    Engine Engine);
