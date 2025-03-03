using Microsoft.Extensions.DependencyInjection;
using RuiSantos.AutoFill.Infrastructure.Engines.Core;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Engines;

public static class ServiceCollectionExtensions
{
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