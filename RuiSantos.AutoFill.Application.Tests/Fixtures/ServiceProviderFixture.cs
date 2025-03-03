using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RuiSantos.AutoFill.Application.Tests.Factories;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Clients;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb;

namespace RuiSantos.AutoFill.Application.Tests.Fixtures;

public sealed class ServiceProviderFixture : IDisposable
{
    public IDataContext DataContext { get; }

    private readonly IServiceProvider _provider;

    public ServiceProviderFixture()
    {
        const string connectionString = "Filename=:memory:";
        
        var configuration = ConfigurationFactory.Create();
        var geminiSettings = configuration.GetSection("Engine:Gemini").Get<GeminiSettings>() ?? new GeminiSettings();

        _provider = new ServiceCollection()
            .AddLogging(loggingBuilder => loggingBuilder.AddDebug())
            .UseAutoFill()
            .UseLiteDb(connectionString)
            .UseGeminiEngine(geminiSettings)
            .BuildServiceProvider();

        this.DataContext = _provider.GetRequiredService<IDataContext>();
    }

    public TService GetService<TService>() where TService : class
        => _provider.GetRequiredService<TService>();

    public void Dispose()
    {
        DataContext.Dispose();
    }
}