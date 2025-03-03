using System.Linq.Expressions;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Interface for data context operations.
/// </summary>
public interface IDataContext : IDisposable
{
    /// <summary>
    /// Finds an entity by its identifier.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    Task<T?> FindAsync<T>(Guid id);

    /// <summary>
    /// Finds all entities that match the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type of the entities.</typeparam>
    /// <param name="predicate">The predicate to filter the entities.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities that match the predicate.</returns>
    Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>>? predicate);

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the created entity.</returns>
    Task<Guid> CreateAsync<T>(T entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync<T>(T entity);

    /// <summary>
    /// Deletes an entity by its identifier.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync<T>(Guid id);
}