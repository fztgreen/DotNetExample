using DotNetExample.DataAccess.Wrappers;

using FluentAssertions;

using RandomTestValues;

namespace DotNetExample.IntegrationTests.Wrappers;

public abstract class DocumentDatabaseWrapperContract
{
    private const int ConcurrentReplacements = 10_000;

    private string _collectionName;

    protected IDocumentDatabaseWrapper Wrapper { get; private set; }

    [TestInitialize]
    public void CreateWrapperWithEmptyCollection()
    {
        Wrapper = CreateWrapper();
        _collectionName = RandomValue.String(16);
    }

    [TestMethod]
    public void FindTheInsertedDocument()
    {
        // Arrange
        var document = RandomDocument();
        Wrapper.Insert(_collectionName, document.Id, document);

        // Act
        var found = Wrapper.Find<TestDocument>(_collectionName, document.Id);

        // Assert
        found.Should().Be(document);
    }

    [TestMethod]
    public void ReturnNull_WhenFindingAMissingDocument()
    {
        // Arrange
        var missingId = RandomValue.Guid();

        // Act
        var found = Wrapper.Find<TestDocument>(_collectionName, missingId);

        // Assert
        found.Should().BeNull();
    }

    [TestMethod]
    public void ReturnNull_WhenTheDocumentIsInAnotherCollection()
    {
        // Arrange
        var document = RandomDocument();
        Wrapper.Insert(RandomValue.String(16), document.Id, document);

        // Act
        var found = Wrapper.Find<TestDocument>(_collectionName, document.Id);

        // Assert
        found.Should().BeNull();
    }

    [TestMethod]
    public void FindAllDocumentsInTheCollectionOnly()
    {
        // Arrange
        TestDocument[] documents = [RandomDocument(), RandomDocument(), RandomDocument()];
        InsertAll(documents);
        var documentInAnotherCollection = RandomDocument();
        Wrapper.Insert(RandomValue.String(16), documentInAnotherCollection.Id, documentInAnotherCollection);

        // Act
        var found = Wrapper.FindAll<TestDocument>(_collectionName);

        // Assert
        found.Should().BeEquivalentTo(documents);
    }

    [TestMethod]
    public void FindNoDocuments_WhenTheCollectionIsEmpty()
    {
        // Arrange
        var emptyCollectionName = _collectionName;

        // Act
        var found = Wrapper.FindAll<TestDocument>(emptyCollectionName);

        // Assert
        found.Should().BeEmpty();
    }

    [TestMethod]
    public void Throw_WhenInsertingADuplicateId()
    {
        // Arrange
        var document = RandomDocument();
        Wrapper.Insert(_collectionName, document.Id, document);
        var duplicate = document with { Value = RandomValue.String() };

        // Act
        var insertingDuplicate = Wrapper.Invoking(wrapper => wrapper.Insert(_collectionName, duplicate.Id, duplicate));

        // Assert
        insertingDuplicate.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void ReplaceTheDocument_WhenItExists()
    {
        // Arrange
        var document = RandomDocument();
        Wrapper.Insert(_collectionName, document.Id, document);
        var replacement = document with { Value = RandomValue.String() };

        // Act
        var replaced = Wrapper.Replace(_collectionName, document.Id, replacement);

        // Assert
        replaced.Should().BeTrue();
        Wrapper.Find<TestDocument>(_collectionName, document.Id).Should().Be(replacement);
    }

    [TestMethod]
    public void ReturnFalseWithoutInserting_WhenReplacingAMissingDocument()
    {
        // Arrange
        var document = RandomDocument();

        // Act
        var replaced = Wrapper.Replace(_collectionName, document.Id, document);

        // Assert
        replaced.Should().BeFalse();
        Wrapper.Find<TestDocument>(_collectionName, document.Id).Should().BeNull();
    }

    [TestMethod]
    public void ReplaceEveryTime_WhenReplacedConcurrently()
    {
        // Arrange
        var document = RandomDocument();
        Wrapper.Insert(_collectionName, document.Id, document);
        var replacements = RandomReplacementsFor(document);
        var results = new bool[ConcurrentReplacements];

        // Act
        Parallel.For(0, ConcurrentReplacements, index => results[index] = Wrapper.Replace(_collectionName, document.Id, replacements[index]));

        // Assert
        results.Should().NotContain(false);
    }

    [TestMethod]
    public void DeleteTheDocument_WhenItExists()
    {
        // Arrange
        var document = RandomDocument();
        Wrapper.Insert(_collectionName, document.Id, document);

        // Act
        var deleted = Wrapper.Delete(_collectionName, document.Id);

        // Assert
        deleted.Should().BeTrue();
        Wrapper.Find<TestDocument>(_collectionName, document.Id).Should().BeNull();
    }

    [TestMethod]
    public void ReturnFalse_WhenDeletingAMissingDocument()
    {
        // Arrange
        var missingId = RandomValue.Guid();

        // Act
        var deleted = Wrapper.Delete(_collectionName, missingId);

        // Assert
        deleted.Should().BeFalse();
    }

    protected abstract IDocumentDatabaseWrapper CreateWrapper();

    private static TestDocument RandomDocument()
    {
        return new TestDocument(RandomValue.Guid(), RandomValue.String());
    }

    private static TestDocument[] RandomReplacementsFor(TestDocument document)
    {
        var replacements = new TestDocument[ConcurrentReplacements];
        for (var index = 0; index < ConcurrentReplacements; index++)
        {
            replacements[index] = document with { Value = RandomValue.String() };
        }

        return replacements;
    }

    private void InsertAll(TestDocument[] documents)
    {
        foreach (var document in documents)
        {
            Wrapper.Insert(_collectionName, document.Id, document);
        }
    }
}
