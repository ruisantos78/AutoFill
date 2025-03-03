using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

/// <summary>
/// Interface for engine operations service.
/// </summary>
public interface IEngineOperationsService
{
    /// <summary>
    /// Detects fields and values in the provided document text.
    /// </summary>
    /// <param name="documentText">The text of the document to analyze.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of detected fields.</returns>
    Task<List<DetectedField>> DetectFieldsAndValuesAsync(string documentText);

    /// <summary>
    /// Converts the provided document to Markdown format.
    /// </summary>
    /// <param name="file">The StreamReader of the document to convert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Markdown formatted string.</returns>
    Task<string> ConvertDocumentToMarkdownAsync(StreamReader file);
}