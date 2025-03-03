using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Infrastructure.Engines.Contracts;
using RuiSantos.AutoFill.Infrastructure.Engines.Core;
using RuiSantos.AutoFill.Infrastructure.Engines.Factories;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Services;

public abstract class EngineOperationsServiceBase<TSettings>(
        IEngineClient engine, 
        IOptions<TSettings> options, 
        ILogger logger
    ) : IEngineOperationsService where TSettings : EngineSettingsBase
{
    protected TSettings Settings => options.Value;
    
    public async Task<List<DetectedField>> DetectFieldsAndValuesAsync(string documentText)
    {
        try
        {
            logger.LogInformation("Sending document text to {ModelName} for field and value detection.",
                Settings.ModelName);

            var prompt = await PromptFactory.GetDetectFieldsAndValuesAsync(documentText);

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
}