using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Application.Factories;

internal static class TemplateDocumentFactory
{
    public static TemplateDocument CreateFromFilePath(string filePath, string content, IEnumerable<TemplateField> fields)
    {
        var documentName = Path.GetFileNameWithoutExtension(filePath);
        
        return new TemplateDocument(
            Guid.NewGuid(), 
            documentName, 
            content, 
            fields.ToArray());
    }
}