using MongoDB.Driver;

namespace TdmPrototypeDmpSynchroniser.Data;

public interface IMongoDbClientFactory
{
    protected IMongoClient CreateClient();

    IMongoCollection<T> GetCollection<T>(string collection);
}