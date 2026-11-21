using System.Reflection;

using FluentAssertions;

namespace DotNetExample.Tests.Architecture;

[TestClass]
public class LayersShould
{
    [TestMethod]
    public void KeepDomainFreeOfDependencies()
    {
        // Arrange
        var domainReferences = SolutionAssemblies.Domain.GetReferencedAssemblies();

        // Act
        var nonSystemReferences = NamesNotStartingWith(domainReferences, "System");

        // Assert
        ViolationReport.Of(nonSystemReferences).Should().BeEmpty();
    }

    [TestMethod]
    public void KeepApiIndependentOfDataAccessAndIoC()
    {
        // Arrange
        var apiReferences = SolutionAssemblies.Api.GetReferencedAssemblies();

        // Act
        var forbiddenReferences = NamesOf(apiReferences, SolutionAssemblies.DataAccess, SolutionAssemblies.IoC);

        // Assert
        ViolationReport.Of(forbiddenReferences).Should().BeEmpty();
    }

    [TestMethod]
    public void KeepDataAccessIndependentOfApiAndIoC()
    {
        // Arrange
        var dataAccessReferences = SolutionAssemblies.DataAccess.GetReferencedAssemblies();

        // Act
        var forbiddenReferences = NamesOf(dataAccessReferences, SolutionAssemblies.Api, SolutionAssemblies.IoC);

        // Assert
        ViolationReport.Of(forbiddenReferences).Should().BeEmpty();
    }

    private static List<string> NamesNotStartingWith(AssemblyName[] references, string allowedPrefix)
    {
        var names = new List<string>();
        foreach (var reference in references)
        {
            if (!reference.Name!.StartsWith(allowedPrefix))
            {
                names.Add(reference.Name);
            }
        }

        return names;
    }

    private static List<string> NamesOf(AssemblyName[] references, params Assembly[] forbiddenAssemblies)
    {
        var names = new List<string>();
        foreach (var reference in references)
        {
            foreach (var forbiddenAssembly in forbiddenAssemblies)
            {
                if (reference.Name == forbiddenAssembly.GetName().Name)
                {
                    names.Add(reference.Name!);
                }
            }
        }

        return names;
    }
}
