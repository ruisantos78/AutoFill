using RuiSantos.AutoFill.Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.Mongo.Mappings;

internal class TemplateDocumentMapper
{
    /// <summary>
    /// Gets the unique identifier for the document.
    /// </summary>
    [BsonId(IdGenerator = typeof(GuidGenerator))]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    /// <summary>
    /// Gets the date and time when the document was last updated.
    /// </summary>
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; init; } = DateTime.Now;
    
    /// <summary>
    /// Gets the name of the document.
    /// </summary>
    [BsonElement("name")]
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets the content of the document.
    /// </summary>
    [BsonElement("content")]
    public string Content { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets the list of fields associated with the document.
    /// </summary>
    [BsonElement("fields")]
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