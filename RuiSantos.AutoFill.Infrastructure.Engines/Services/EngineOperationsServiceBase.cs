using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Infrastructure.Engines.Clients;
using RuiSantos.AutoFill.Infrastructure.Engines.Factories;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Engines.Services.Contracts;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Services;

/// <summary>
/// Base class for engine operations services.
/// </summary>
/// <typeparam name="TSettings">The type of settings used by the engine.</typeparam>
/// <param name="engine">The engine client used for executing prompts.</param>
/// <param name="options">The options containing the settings for the engine.</param>
/// <param name="logger">The logger used for logging information and errors.</param>
public abstract class EngineOperationsServiceBase<TSettings>(
        IEngineClient engine, 
        IOptions<TSettings> options, 
        ILogger logger
    ) : IEngineOperationsService where TSettings : EngineSettingsBase
{
    /// <summary>
    /// Gets the settings for the engine.
    /// </summary>
    protected TSettings Settings => options.Value;
    
    /// <summary>
    /// Detects fields and values in the provided document text.
    /// </summary>
    /// <param name="documentText">The text of the document to analyze.</param>
    /// <returns>A list of detected fields and their values.</returns>
    public virtual async Task<List<TemplateField>> DetectFieldsAndValuesAsync(string documentText)
    {
        try
        {
            logger.LogInformation("Sending document text to {ModelName} for field and value detection.",
                Settings.ModelName);

            var prompt = await PromptFactory.DetectFieldsAndValuesAsync(documentText);

            var detectedFields = await engine.ExecutePromptAsync<DetectedFieldResponse[]>(prompt);
            return detectedFields?.Select(f => f.ToDomain()).ToList() ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error detecting fields with  {ModelName}", 
                Settings.ModelName);
            
            throw;
        }
    }

    /// <summary>
    /// Converts the provided document to Markdown format.
    /// </summary>
    /// <param name="fileName">The name of the file to convert.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual async Task<string> ConvertDocumentToMarkdownAsync(string fileName)
    {
        try
        {
            logger.LogInformation("Sending document text to {ModelName} for convert to markdown.", 
                Settings.ModelName);

            var prompt = await PromptFactory.ConvertDocumentToMarkdownAsync();
            
            return await engine.UploadFileAndExecuteAsync(prompt, fileName);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error converting document to markdown with {ModelName}", 
                Settings.ModelName);
            throw;
        }
    }
}