using Microsoft.Extensions.DependencyInjection;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseRepository<TDataContext>(this IServiceCollection services) 
        where TDataContext: class, IDataContext
    {
        if (services.Any(x => x.ServiceType == typeof(IDataContext)))
            throw new InvalidOperationException("Data context service has already been registered");
        
        services.AddScoped<IDataContext, TDataContext>();
        
        return services;
    }
}