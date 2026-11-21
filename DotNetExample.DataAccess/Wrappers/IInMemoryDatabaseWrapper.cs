namespace DotNetExample.DataAccess.Wrappers;

public interface IInMemoryDatabaseWrapper
{
    T? Find<T>(string collectionName, Guid id) where T : class;

    IReadOnlyCollection<T> FindAll<T>(string collectionName);

    void Insert<T>(string collectionName, Guid id, T document) where T : class;

    bool Replace<T>(string collectionName, Guid id, T document) where T : class;

    bool Delete(string collectionName, Guid id);
}
