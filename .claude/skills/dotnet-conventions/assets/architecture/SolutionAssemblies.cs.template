using System.Reflection;

using ArchUnitNET.Loader;

using DotNetExample.Api;
using DotNetExample.DataAccess.Wrappers;
using DotNetExample.Domain.Entities.Cars;

namespace DotNetExample.Tests.Architecture;

public static class SolutionAssemblies
{
    public static readonly Assembly Domain = typeof(Car).Assembly;
    public static readonly Assembly DataAccess = typeof(IDocumentDatabaseWrapper).Assembly;
    public static readonly Assembly Api = typeof(ApiConfiguration).Assembly;
    public static readonly Assembly IoC = typeof(Program).Assembly;
    public static readonly Assembly Tests = typeof(SolutionAssemblies).Assembly;

    public static readonly Assembly[] Production = [Domain, DataAccess, Api, IoC];
    public static readonly Assembly[] All = [.. Production, Tests];

    public static readonly ArchUnitNET.Domain.Architecture Architecture = new ArchLoader().LoadAssemblies(All).Build();
}
