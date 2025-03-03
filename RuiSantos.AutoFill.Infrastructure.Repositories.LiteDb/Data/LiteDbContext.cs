using System.Linq.Expressions;
using LiteDB.Async;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Data;

/// <summary>
/// Represents the LiteDbContext which provides methods to interact with the LiteDB database.
/// </summary>
/// <param name="settings">The LiteDbSettings options.</param>
internal sealed class LiteDbContext(IOptions<LiteDbSettings> settings) : IDataContext
{
    private readonly ILiteDatabaseAsync _database = new LiteDatabaseAsync(settings.Value.ConnectionString);

    /// <summary>
    /// Disposes the LiteDbContext and releases all resources.
    /// </summary>
    public void Dispose()
    {
        _database.Dispose();
    }

    /// <summary>
    /// Finds an entity by its ID.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    public Task<T?> FindAsync<T>(Guid id)
    {
        return _database.GetCollection<T?>().FindByIdAsync(id);
    }

    /// <summary>
    /// Finds all entities that match the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the entities.</typeparam>
    /// <param name="predicate">The predicate to filter the entities. If null, all entities are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    public Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>>? predicate)
    {
        if (predicate is null)
            return _database.GetCollection<T>().FindAllAsync();
        
        return _database.GetCollection<T>().FindAsync(predicate);
    }

    /// <summary>
    /// Creates a new entity in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the created entity.</returns>
    public async Task<Guid> CreateAsync<T>(T entity)
    {
        return await _database.GetCollection<T>().InsertAsync(entity);
    }

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateAsync<T>(T entity)
    {
        await _database.GetCollection<T>().UpdateAsync(entity);
    }

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteAsync<T>(Guid id)
    {
        await _database.GetCollection<T>().DeleteAsync(id);
    }
}