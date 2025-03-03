using LiteDB;
using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Mappings;

/// <summary>
/// Represents a mapper for the TemplateDocument entity in LiteDB.
/// </summary>
internal class TemplateDocumentMapper
{
    /// <summary>
    /// Gets the unique identifier for the document.
    /// </summary>
    [BsonId]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    /// <summary>
    /// Gets the date and time when the document was last updated.
    /// </summary>
    [BsonField("updated_at")]
    public DateTime UpdatedAt { get; init; } = DateTime.Now;
    
    /// <summary>
    /// Gets the name of the document.
    /// </summary>
    [BsonField("name")]
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets the content of the document.
    /// </summary>
    [BsonField("content")]
    public string Content { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets the list of fields associated with the document.
    /// </summary>
    [BsonField("fields")]
    public List<TemplateFieldsMapper> Fields { get; init; } = [];

    /// <summary>
    /// Converts the mapper to a domain entity.
    /// </summary>
    /// <returns>A TemplateDocument domain entity.</returns>
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