using System.Reflection;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

namespace DotNetExample.Tests.Architecture;

[TestClass]
public class CodeShould
{
    private const string AnonymousTypePrefix = "<>f__AnonymousType";
    private const string LambdaBodyMarker = ">b__";

    private const BindingFlags EveryDeclaredMethod =
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

    private static readonly string[] _bannedNamespaces = ["Newtonsoft.Json", "Microsoft.EntityFrameworkCore"];

    [TestMethod]
    public void NotUseBannedLibraries()
    {
        // Arrange
        var noExemptNamespace = string.Empty;

        // Act
        var violations = NamespaceDependencies.Find(SolutionAssemblies.All, _bannedNamespaces, noExemptNamespace);

        // Assert
        violations.Should().BeEmpty();
    }

    [TestMethod]
    public void NotUseAnonymousTypes()
    {
        // Arrange
        var assemblies = SolutionAssemblies.All;

        // Act
        var anonymousTypes = AnonymousTypesIn(assemblies);

        // Assert
        anonymousTypes.Should().BeEmpty();
    }

    [TestMethod]
    public void NotUseLambdas()
    {
        // Arrange
        var assemblies = SolutionAssemblies.All;

        // Act
        var lambdas = LambdasIn(assemblies);

        // Assert
        lambdas.Should().BeEmpty();
    }

    [TestMethod]
    public void NameAsyncMethodsWithAsyncSuffix()
    {
        // Arrange
        var assemblies = SolutionAssemblies.Production;

        // Act
        var methodsMissingSuffix = AsyncMethodsWithoutSuffixIn(assemblies);

        // Assert
        methodsMissingSuffix.Should().BeEmpty();
    }

    private static List<string> AsyncMethodsWithoutSuffixIn(Assembly[] assemblies)
    {
        var methods = new List<string>();
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!IsCompilerGenerated(type.Name) && !IsController(type))
                {
                    methods.AddRange(AsyncMethodsWithoutSuffixOn(type));
                }
            }
        }

        return methods;
    }

    private static IEnumerable<string> AsyncMethodsWithoutSuffixOn(Type type)
    {
        foreach (var method in type.GetMethods(EveryDeclaredMethod))
        {
            if (IsAwaitable(method.ReturnType) && !method.Name.EndsWith("Async") && !IsCompilerGenerated(method.Name))
            {
                yield return $"{type.FullName}.{method.Name}";
            }
        }
    }

    private static bool IsAwaitable(Type returnType)
    {
        var definition = returnType.IsGenericType ? returnType.GetGenericTypeDefinition() : returnType;
        return definition == typeof(Task) || definition == typeof(Task<>) || definition == typeof(ValueTask) || definition == typeof(ValueTask<>);
    }

    private static bool IsController(Type type)
    {
        return typeof(ControllerBase).IsAssignableFrom(type);
    }

    private static bool IsCompilerGenerated(string name)
    {
        return name.StartsWith('<');
    }

    private static List<string> AnonymousTypesIn(Assembly[] assemblies)
    {
        var anonymousTypes = new List<string>();
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.Name.StartsWith(AnonymousTypePrefix))
                {
                    anonymousTypes.Add($"{assembly.GetName().Name}: {type.Name}");
                }
            }
        }

        return anonymousTypes;
    }

    private static List<string> LambdasIn(Assembly[] assemblies)
    {
        var lambdas = new List<string>();
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(EveryDeclaredMethod))
                {
                    if (method.Name.Contains(LambdaBodyMarker))
                    {
                        lambdas.Add($"{OutermostType(type).FullName}: {method.Name}");
                    }
                }
            }
        }

        return lambdas;
    }

    private static Type OutermostType(Type type)
    {
        while (type.DeclaringType is not null)
        {
            type = type.DeclaringType;
        }

        return type;
    }
}
