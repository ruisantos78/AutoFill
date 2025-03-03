using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Domain.Interfaces;

public interface ITemplateManagerService
{
    Task<TemplateDocument> GenerateTemplateFromDocumentAsync(string documentName, string documentText);
}