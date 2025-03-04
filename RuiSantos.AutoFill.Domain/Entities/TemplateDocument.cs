namespace RuiSantos.AutoFill.Domain.Entities;

/// <summary>
/// Represents a template document with an ID, name, content, and detected fields.
/// </summary>
/// <param name="Id">The unique identifier of the template document.</param>
/// <param name="Name">The name of the template document.</param>
/// <param name="Content">The content of the template document.</param>
/// <param name="Fields">The detected fields within the template document.</param>
public record TemplateDocument(
    Guid Id, 
    string Name, 
    string Content, 
    TemplateField[] Fields);