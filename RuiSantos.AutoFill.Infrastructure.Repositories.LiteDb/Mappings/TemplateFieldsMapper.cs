using LiteDB;
using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Mappings;

/// <summary>
/// Represents a mapper for template fields in LiteDB.
/// </summary>
internal class TemplateFieldsMapper
{
    /// <summary>
    /// Gets the name of the template field.
    /// </summary>
    [BsonField("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the value of the template field.
    /// </summary>
    [BsonField("value")]
    public string Value { get; init; } = string.Empty;

    /// <summary>
    /// Gets the label of the template field.
    /// </summary>
    [BsonField("label")]
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// Converts the current instance to a <see cref="DetectedField"/> domain object.
    /// </summary>
    /// <returns>A <see cref="DetectedField"/> object.</returns>
    public DetectedField ToDomain()
    {
        return new DetectedField(Value, Name, Label);
    }
}