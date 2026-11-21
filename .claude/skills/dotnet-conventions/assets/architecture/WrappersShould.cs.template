using System.Reflection;
using System.Runtime.CompilerServices;

using DotNetExample.DataAccess.Wrappers;

using FluentAssertions;

namespace DotNetExample.IntegrationTests.Architecture;

[TestClass]
public class WrappersShould
{
    private const string IntegrationTestSuffix = "Should";

    [TestMethod]
    public void HaveAnIntegrationTest()
    {
        // Arrange
        var wrapperImplementations = WrapperImplementations();

        // Act
        var untestedWrappers = WithoutIntegrationTest(wrapperImplementations);

        // Assert
        string.Join(Environment.NewLine, untestedWrappers).Should().BeEmpty();
    }

    private static List<Type> WrapperImplementations()
    {
        var wrappersNamespace = typeof(IDocumentDatabaseWrapper).Namespace;
        var implementations = new List<Type>();
        foreach (var type in typeof(IDocumentDatabaseWrapper).Assembly.GetTypes())
        {
            if (type.Namespace == wrappersNamespace && type.IsClass && !type.IsAbstract && !type.IsDefined(typeof(CompilerGeneratedAttribute)))
            {
                implementations.Add(type);
            }
        }

        return implementations;
    }

    private static List<string> WithoutIntegrationTest(List<Type> wrapperImplementations)
    {
        var testClassNames = TestClassNames();
        var untested = new List<string>();
        foreach (var wrapper in wrapperImplementations)
        {
            if (!testClassNames.Contains($"{wrapper.Name}{IntegrationTestSuffix}"))
            {
                untested.Add($"{wrapper.FullName} has no {wrapper.Name}{IntegrationTestSuffix} test class");
            }
        }

        return untested;
    }

    private static HashSet<string> TestClassNames()
    {
        var names = new HashSet<string>();
        foreach (var type in typeof(WrappersShould).Assembly.GetTypes())
        {
            if (type.IsDefined(typeof(TestClassAttribute)))
            {
                names.Add(type.Name);
            }
        }

        return names;
    }
}
