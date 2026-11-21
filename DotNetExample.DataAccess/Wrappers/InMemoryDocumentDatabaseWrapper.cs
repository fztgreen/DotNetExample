using System.Collections.Concurrent;

namespace DotNetExample.DataAccess.Wrappers;

public class InMemoryDocumentDatabaseWrapper : IDocumentDatabaseWrapper
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, object>> _collections = new();

    public T? Find<T>(string collectionName, Guid id) where T : class
    {
        return Collection(collectionName).TryGetValue(id, out var document) ? document as T : null;
    }

    public IReadOnlyCollection<T> FindAll<T>(string collectionName)
    {
        return Collection(collectionName).Values.OfType<T>().ToArray();
    }

    public void Insert<T>(string collectionName, Guid id, T document) where T : class
    {
        if (!Collection(collectionName).TryAdd(id, document))
        {
            throw new InvalidOperationException($"A document with id {id} already exists in {collectionName}.");
        }
    }

    public bool Replace<T>(string collectionName, Guid id, T document) where T : class
    {
        return Collection(collectionName).TryGetValue(id, out var existingDocument)
            && Collection(collectionName).TryUpdate(id, document, existingDocument);
    }

    public bool Delete(string collectionName, Guid id)
    {
        return Collection(collectionName).TryRemove(id, out _);
    }

    private ConcurrentDictionary<Guid, object> Collection(string collectionName)
    {
        return _collections.GetOrAdd(collectionName, _ => new ConcurrentDictionary<Guid, object>());
    }
}
