using System.Reflection;
using System.Runtime.CompilerServices;

using FluentAssertions;

namespace DotNetExample.Tests.Architecture;

[TestClass]
public class FeatureFoldersShould
{
    private static readonly string[] _featureFolderRoots =
    [
        "DotNetExample.Domain.Commands",
        "DotNetExample.Domain.Queries",
        "DotNetExample.Domain.DataAccess",
        "DotNetExample.Domain.Entities",
        "DotNetExample.Domain.Exceptions",
        "DotNetExample.DataAccess",
        "DotNetExample.Api.Models"
    ];

    [TestMethod]
    public void HoldEveryTypeInsteadOfTheFolderRoot()
    {
        // Arrange
        var assemblies = SolutionAssemblies.Production;

        // Act
        var typesOutsideFeatureFolders = TypesDirectlyInFeatureFolderRoots(assemblies);

        // Assert
        ViolationReport.Of(typesOutsideFeatureFolders).Should().BeEmpty();
    }

    private static List<string> TypesDirectlyInFeatureFolderRoots(Assembly[] assemblies)
    {
        var types = new List<string>();
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsDefined(typeof(CompilerGeneratedAttribute)) && _featureFolderRoots.Contains(type.Namespace))
                {
                    types.Add(type.FullName!);
                }
            }
        }

        return types;
    }
}
