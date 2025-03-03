using System.Linq.Expressions;
using LiteDB.Async;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Core;

internal sealed class LiteDbContext(IOptions<LiteDbSettings> settings) : IDataContext
{
    private readonly ILiteDatabaseAsync _database = new LiteDatabaseAsync(settings.Value.ConnectionString);

    public void Dispose()
    {
        _database.Dispose();
    }

    public Task<T?> FindAsync<T>(Guid id)
    {
        return _database.GetCollection<T?>().FindByIdAsync(id);
    }

    public Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>>? predicate)
    {
        if (predicate is null)
            return _database.GetCollection<T>().FindAllAsync();
        
        return _database.GetCollection<T>().FindAsync(predicate);
    }

    public async Task<Guid> CreateAsync<T>(T entity)
    {
        return await _database.GetCollection<T>().InsertAsync(entity);
    }

    public async Task UpdateAsync<T>(T entity)
    {
        await _database.GetCollection<T>().UpdateAsync(entity);
    }

    public async Task DeleteAsync<T>(Guid id)
    {
        await _database.GetCollection<T>().DeleteAsync(id);
    }
}