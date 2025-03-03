using LiteDB;
using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Mappings;

internal class TemplateDocumentMapper
{
    [BsonId]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [BsonField("updated_at")]
    public DateTime UpdatedAt { get; init; } = DateTime.Now;
    
    [BsonField("name")]
    public string Name { get; init; } = string.Empty;
    
    [BsonField("content")]
    public string Content { get; init; } = string.Empty;
    
    [BsonField("fields")]
    public List<TemplateFieldsMapper> Fields { get; init; } = [];

    public TemplateDocument ToDomain()
    {
        return new TemplateDocument(
            Id,
            Name,
            Content,
            Fields.Select(x => x.ToDomain()).ToArray()
        );
    }
}