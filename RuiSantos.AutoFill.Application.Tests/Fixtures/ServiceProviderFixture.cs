using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RuiSantos.AutoFill.Application.Tests.Factories;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Clients;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb;

namespace RuiSantos.AutoFill.Application.Tests.Fixtures;

public sealed class ServiceProviderFixture: IDisposable
{
    public IDataContext DataContext { get; }

    private readonly IServiceProvider _provider;
    
    public ServiceProviderFixture()
    {
        var options = new GeminiSettings();
        
        var configuration = ConfigurationFactory.Create();
        configuration.GetSection("Engine:Gemini").Bind(options);
        
        _provider = new ServiceCollection()
            .AddLogging(loggingBuilder => loggingBuilder.AddDebug())
            .UseAutoFill()
            .UseLiteDb("Filename=:memory:")
            .UseGeminiEngine(options)
            .BuildServiceProvider();
        
        this.DataContext =  _provider.GetRequiredService<IDataContext>();
    }
    
    public TService GetService<TService>() where TService : class 
        => _provider.GetRequiredService<TService>();
    
    public void Dispose()
    {
        DataContext.Dispose(); 
    }
}