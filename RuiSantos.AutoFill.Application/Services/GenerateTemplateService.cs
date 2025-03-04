using Microsoft.Extensions.Logging;
using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Application.Factories;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Domain.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

namespace RuiSantos.AutoFill.Application.Services;

/// <summary>
/// Service for generate template documents.
/// </summary>
/// <param name="engineOperationsService">Service for engine operations.</param>
/// <param name="templateDocumentRepository">Repository for template documents.</param>
internal class GenerateTemplateService(
    IEngineOperationsService engineOperationsService,
    ITemplateDocumentRepository templateDocumentRepository,
    ILogger<GenerateTemplateService> logger)
    : IGenerateTemplateService
{
    public async Task<TemplateDocument> ExtractFromFileAsync(string fileName, Stream stream)
    {
        logger.LogInformation("Uploading file {fileName}", fileName);
        try
        {
            var markdown = await ConvertFileToMarkdownAsync(fileName, stream);
            var fields = await DetectFieldsAsync(markdown);
            var template = await CreateAndStoreTemplateAsync(fileName, markdown, fields);
            
            logger.LogDebug("Template {templateName} uploaded successfully", template.Name);
            return template;        
        }
        catch (ApplicationServiceException exception)
        {
            logger.LogWarning(exception, "Was not possible to uploading file {fileName} due: {message}", fileName, exception.Message);
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error uploading file {fileName}", fileName);
            throw ExceptionsFactory.UnexpectedError(exception);
        }
    }
    
    private async Task<string> ConvertFileToMarkdownAsync(string fileName, Stream stream)
    {
        logger.LogDebug("Converting file {fileName} to markdown", fileName);
        
        var markdown = await engineOperationsService.ConvertDocumentToMarkdownAsync(fileName, stream);
        if (string.IsNullOrWhiteSpace(markdown))
            throw ExceptionsFactory.MarkdownConversionFailed(fileName);
        
        return markdown;
    }
    
    private async Task<IReadOnlyList<TemplateField>> DetectFieldsAsync(string markdown)
    {
        logger.LogDebug("Detecting fields and values in markdown");
        
        var fields = await engineOperationsService.DetectFieldsAndValuesAsync(markdown);
        if (fields.Count == 0)
            throw ExceptionsFactory.FieldsAreNotDetectedByEngine();
        
        return fields;
    }
    
    private async Task<TemplateDocument> CreateAndStoreTemplateAsync(string filePath, string markdown, IReadOnlyList<TemplateField> fields)
    {
        logger.LogDebug("Creating and storing template from file {filePath}", filePath);
        
        var template = TemplateDocumentFactory.CreateFromFilePath(filePath, markdown, fields);
        await templateDocumentRepository.CreateAsync(template);
        
        return template;
    }
}