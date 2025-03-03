using Microsoft.Extensions.DependencyInjection;
using RuiSantos.AutoFill.Infrastructure.Engines.Clients;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Engines;

/// <summary>
/// Extension methods for IServiceCollection to register engine clients.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the specified engine client type in the service collection.
    /// </summary>
    /// <typeparam name="TEngine">The type of the engine client to register.</typeparam>
    /// <param name="services">The service collection to add the engine client to.</param>
    /// <returns>The updated service collection.</returns>
    /// <exception cref="InvalidOperationException">Thrown if an engine client service has already been registered.</exception>
    public static IServiceCollection UseEngine<TEngine>(this IServiceCollection services) 
        where TEngine: class, IEngineClient
    {
        if (services.Any(x => x.ServiceType == typeof(IEngineClient)))
            throw new InvalidOperationException("Engine client service has already been registered");

        services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
        services.AddScoped<IEngineClient, TEngine>();
        
        return services;
    }
}