using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Domain.Interfaces;

/// <summary>
/// Interface for managing template operations.
/// </summary>
public interface ITemplateManagerService
{
    /// <summary>
    /// Generates a template document from the provided document name and text.
    /// </summary>
    /// <param name="documentName">The name of the document.</param>
    /// <param name="documentText">The text content of the document.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generated TemplateDocument.</returns>
    Task<TemplateDocument> GenerateTemplateFromDocumentAsync(string documentName, string documentText);
    
    /// <summary>
    /// Converts a document into Markdown format.
    /// </summary>
    /// <param name="filePath">The path to the document file.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Markdown formatted string.</returns>
    Task<string> ConvertDocumentIntoMarkdownAsync(string filePath);
}