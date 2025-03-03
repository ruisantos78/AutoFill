using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Core;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Engines.Services;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Services;

public class GeminiOperationsService(
    IEngineClient engineClient,
    IOptions<GeminiSettings> options,
    ILogger<GeminiOperationsService> logger)
    : EngineOperationsServiceBase<GeminiSettings>(engineClient, options, logger);