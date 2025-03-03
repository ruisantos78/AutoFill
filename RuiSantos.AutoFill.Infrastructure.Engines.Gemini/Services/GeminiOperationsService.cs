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
        var markdown = await base.ConvertDocumentToMarkdownAsync(fileName);
        
        if (markdown.StartsWith("```markdown"))
            markdown = markdown.Remove(0, 11);
    
        if (markdown.EndsWith("```"))
            markdown = markdown.Remove(markdown.Length - 3);
        
        return markdown.TrimEnd();
    }
}