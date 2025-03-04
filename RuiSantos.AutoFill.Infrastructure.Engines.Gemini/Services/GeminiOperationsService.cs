using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Clients;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Engines.Services;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Services;

/// <summary>
/// Service for performing operations with the Gemini engine.
/// </summary>
/// <param name="engineClient">The client used to interact with the engine.</param>
/// <param name="options">The settings for the Gemini engine.</param>
/// <param name="logger">The logger instance for logging operations.</param>
public class GeminiOperationsService(
    IEngineClient engineClient,
    IOptions<GeminiSettings> options,
    ILogger<GeminiOperationsService> logger)
    : EngineOperationsServiceBase<GeminiSettings>(engineClient, options, logger)
{
    /// <summary>
    /// Converts a document to Markdown format.
    /// </summary>
    /// <param name="fileName">The name of the file to convert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the converted Markdown string.</returns>
    public override async Task<string> ConvertDocumentToMarkdownAsync(string fileName)
    {
        var response = await base.ConvertDocumentToMarkdownAsync(fileName);

        response = response.Trim();
        if (response.StartsWith("```markdown\n") && response.EndsWith("```"))
            return response[12..^3];

        return string.Empty;
    }
}