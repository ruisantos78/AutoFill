using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Core;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Services;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
[assembly: InternalsVisibleTo("RuiSantos.AutoFill.Infrastructure.Engines.Tests")]
[assembly: InternalsVisibleTo("RuiSantos.AutoFill.Tests")]

namespace RuiSantos.AutoFill.Infrastructure.Engines.Gemini;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseGeminiEngine(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseEngine<GeminiClient>();
        
        services.Configure<GeminiSettings>(options => configuration.GetSection("Engine:Gemini").Bind(options));
        services.AddScoped<IEngineOperationsService, GeminiOperationsService>();
        
        return services;
    }
}