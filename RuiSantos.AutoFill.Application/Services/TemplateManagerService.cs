using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Domain.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

namespace RuiSantos.AutoFill.Application.Services;

/// <summary>
/// Service for managing template documents.
/// </summary>
/// <param name="engineOperationsService">Service for engine operations.</param>
/// <param name="templateDocumentRepository">Repository for template documents.</param>
internal class TemplateManagerService(
    IEngineOperationsService engineOperationsService,
    ITemplateDocumentRepository templateDocumentRepository)
    : ITemplateManagerService
{
    /// <summary>
    /// Generates a template document from the provided document text.
    /// </summary>
    /// <param name="documentName">The name of the document.</param>
    /// <param name="documentText">The text of the document.</param>
    /// <returns>The generated template document.</returns>
    /// <exception cref="TemplateManagerServiceException">Thrown when no fields are detected by the engine.</exception>
    public async Task<TemplateDocument> GenerateTemplateFromDocumentAsync(string documentName, string documentText)
    { 
        var fields = await engineOperationsService.DetectFieldsAndValuesAsync(documentText);
        if (fields.Count == 0)
            throw new TemplateManagerServiceException(
                ErrorCodes.FieldAreNotDetectByEngine, 
                nameof(GenerateTemplateFromDocumentAsync), 
                "The AI Engine has not detected any fields for this template");
            
        var template = new TemplateDocument(
            Guid.NewGuid(), 
            documentName, 
            documentText, 
            fields.ToArray());
        
        await templateDocumentRepository.CreateAsync(template);
        return template;
    }
}