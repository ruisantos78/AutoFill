namespace RuiSantos.AutoFill.Domain.Entities;

public record TemplateDocument(
    Guid Id, 
    string Name, 
    string Content, 
    DetectedField[] Fields);