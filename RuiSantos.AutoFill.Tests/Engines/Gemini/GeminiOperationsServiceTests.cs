using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Clients;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Services;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

namespace RuiSantos.AutoFill.Tests.Engines.Gemini;

public partial class GeminiOperationsServiceTests
{
    private readonly ILogger<GeminiOperationsService> _logger = Substitute.For<ILogger<GeminiOperationsService>>();
    private readonly IHttpClientFactory _client = Substitute.For<IHttpClientFactory>();
    private readonly IOptions<GeminiSettings> _options = Substitute.For<IOptions<GeminiSettings>>();

    private readonly IEngineOperationsService _service;
    
    public GeminiOperationsServiceTests()
    {
        _options.Value.Returns(new GeminiSettings() { ApiKey = "test_mode" });
        _client.WithBaseUrl(Arg.Any<string>()).Returns(_client);
        
        var engine = new GeminiClient(_options, _client);
        _service = new GeminiOperationsService(engine, _options, _logger);
    }
}