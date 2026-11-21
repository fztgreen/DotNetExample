using DotNetExample.DataAccess.Wrappers;

namespace DotNetExample.IntegrationTests.Wrappers;

[TestClass]
public class InMemoryDocumentDatabaseWrapperShould : DocumentDatabaseWrapperContract
{
    protected override IDocumentDatabaseWrapper CreateWrapper()
    {
        return new InMemoryDocumentDatabaseWrapper();
    }
}
