using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.Mongo.Data;

public sealed class MongoContext(IOptions<MongoClientSettings> settings) : IDataContext
{
    private readonly IMongoDatabase _database = new MongoClient(settings.Value)
        .GetDatabase(settings.Value.ApplicationName ?? "autofill");

    public void Dispose()
    {
        _database.Client.Dispose();
    }

    public async Task<T?> FindAsync<T>(Guid id)
    {
        var collection = _database.GetCollection<T>(typeof(T).Name);
        var filter = Builders<T>.Filter.Eq("Id", id);

        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>>? predicate)
    {
        var collection = _database.GetCollection<T>(typeof(T).Name);

        if (predicate is null)
        {
            return await collection.Find(_ => true).ToListAsync();
        }

        return await collection.Find(predicate).ToListAsync();
    }

    public async Task<Guid> CreateAsync<T>(T entity)
    {
        var collection = _database.GetCollection<T>(typeof(T).Name);
        await collection.InsertOneAsync(entity);

        var idProperty = typeof(T).GetProperty("Id");
        return (Guid?)idProperty?.GetValue(entity) ?? Guid.Empty;
    }

    public async Task UpdateAsync<T>(T entity)
    {
        var collection = _database.GetCollection<T>(typeof(T).Name);

        var idProperty = typeof(T).GetProperty("Id");
        if (idProperty?.GetValue(entity) is Guid id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            await collection.ReplaceOneAsync(filter, entity);
        }
    }

    public async Task DeleteAsync<T>(Guid id)
    {
        var collection = _database.GetCollection<T>(typeof(T).Name);
        var filter = Builders<T>.Filter.Eq("Id", id);
        
        await collection.DeleteOneAsync(filter);
    }
}