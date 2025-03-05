using RuiSantos.AutoFill.Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.Mongo.Mappings;

public class TemplateFieldsMapper
{
    /// <summary>
    /// Gets the name of the template field.
    /// </summary>
    [BsonElement("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the value of the template field.
    /// </summary>
    [BsonElement("value")]
    public string Value { get; init; } = string.Empty;

    /// <summary>
    /// Gets the label of the template field.
    /// </summary>
    [BsonElement("label")]
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// Converts the current instance to a <see cref="TemplateField"/> domain object.
    /// </summary>
    /// <returns>A <see cref="TemplateField"/> object.</returns>
    public TemplateField ToDomain()
    {
        return new TemplateField(Value, Name, Label);
    }
}