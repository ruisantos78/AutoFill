using System.Runtime.CompilerServices;
using LiteDB;
using LiteDB.Async;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Data;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Services;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
[assembly: InternalsVisibleTo("RuiSantos.AutoFill.Application.Tests")]
[assembly: InternalsVisibleTo("RuiSantos.AutoFill.Tests")]

namespace RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb;

/// <summary>
/// Provides extension methods for registering LiteDb services in the <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers LiteDb services and configurations in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configurations">An action to configure the LiteDb settings.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection UseLiteDb(this IServiceCollection services, Action<ConnectionString> configurations)
    {
        var settings = new ConnectionString();
        configurations(settings);

        return services.UseLiteDb(settings);
    }

    /// <summary>
    /// Registers LiteDb services and configurations in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="settings">The LiteDb settings to use.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection UseLiteDb(this IServiceCollection services, ConnectionString settings)
    {
        return services.UseRepository<LiteDbContext>()
            .AddSingleton(Options.Create(settings))
            .AddScoped<ITemplateDocumentRepository, TemplateDocumentRepository>();
    }
}