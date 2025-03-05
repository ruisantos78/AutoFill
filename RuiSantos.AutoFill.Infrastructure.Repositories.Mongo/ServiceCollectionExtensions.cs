using System.Net;
using System.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.Mongo.Data;
using RuiSantos.AutoFill.Infrastructure.Repositories.Mongo.Services;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.Mongo;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers LiteDb services and configurations in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="configurations">An action to configure the LiteDb settings.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection UseMongoDb(this IServiceCollection services, Action<MongoClientSettings> configurations)
    {
        var settings = new MongoClientSettings();
        configurations(settings);

        return services.UseMongoDb(settings);
    }

    /// <summary>
    /// Registers LiteDb services and configurations in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <param name="settings">The LiteDb settings to use.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection UseMongoDb(this IServiceCollection services, MongoClientSettings settings)
    {
        return services.UseRepository<MongoContext>()
            .AddSingleton(Options.Create(settings))
            .AddScoped<ITemplateDocumentRepository, TemplateDocumentRepository>();
    }

    public static SecureString ToSecureString(this string password)
    {
        return new NetworkCredential("", password).SecurePassword;
    }
}