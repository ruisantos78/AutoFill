using Microsoft.Extensions.DependencyInjection;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Repositories;

/// <summary>
/// Extension methods for IServiceCollection to register repositories.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a data context as a scoped service in the IServiceCollection.
    /// </summary>
    /// <typeparam name="TDataContext">The type of the data context to register.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The IServiceCollection with the registered service.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a data context service has already been registered.</exception>
    public static IServiceCollection UseRepository<TDataContext>(this IServiceCollection services) 
        where TDataContext: class, IDataContext
    {
        if (services.Any(x => x.ServiceType == typeof(IDataContext)))
            throw new InvalidOperationException("Data context service has already been registered");
        
        return services.AddScoped<IDataContext, TDataContext>();
    }
}