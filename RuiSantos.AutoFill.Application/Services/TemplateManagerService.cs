using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Domain.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

namespace RuiSantos.AutoFill.Application.Services;

internal class TemplateManagerService(
    IEngineOperationsService engineOperationsService,
    ITemplateDocumentRepository templateDocumentRepository)
    : ITemplateManagerService
{
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