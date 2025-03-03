using System.Linq.Expressions;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

public interface IDataContext: IDisposable
{
    Task<T?> FindAsync<T>(Guid id);
    Task<IEnumerable<T>> FindAllAsync<T>(Expression<Func<T, bool>>? predicate);

    Task<Guid> CreateAsync<T>(T entity);
    Task UpdateAsync<T>(T entity);
    Task DeleteAsync<T>(Guid id);
}