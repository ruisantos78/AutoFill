using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Domain.Interfaces;

/// <summary>
/// Interface for managing template operations.
/// </summary>
public interface IGenerateTemplateService
{
    Task<TemplateDocument> ExtractFromFileAsync(string filePath);  
}