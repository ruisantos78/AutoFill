using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RuiSantos.AutoFill.Application.Tests.Factories;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb;

namespace RuiSantos.AutoFill.Application.Tests.Fixtures;

public sealed class ServiceProviderFixture: IDisposable
{
    public IDataContext DataContext { get; }

    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _provider;
    
    public ServiceProviderFixture()
    {
        _configuration = ConfigurationFactory.Create();
        
        _provider = new ServiceCollection()
            .AddLogging(loggingBuilder => loggingBuilder.AddDebug())
            .UseAutoFill(_configuration)
            .UseLiteDb(_configuration)
            .UseGeminiEngine(_configuration)
            .BuildServiceProvider();
        
        this.DataContext =  _provider.GetRequiredService<IDataContext>();
    }
    
    public TService GetService<TService>() where TService : class 
        => _provider.GetRequiredService<TService>();
    
    private void CleanLiteDbFiles()
    {
        var connectionString = _configuration.GetSection("Repository:LiteDb")
            .GetValue<string>("ConnectionString");
        
        if (string.IsNullOrEmpty(connectionString))
            return;

        var match = Regex.Match(connectionString, @"Filename=(?<filename>[^;]+)", RegexOptions.IgnoreCase);
        if (match.Success && File.Exists(match.Groups["filename"].Value))
            File.Delete(match.Groups["filename"].Value);
    }
    
    public void Dispose()
    {
        DataContext.Dispose(); 
        CleanLiteDbFiles();
    }
}