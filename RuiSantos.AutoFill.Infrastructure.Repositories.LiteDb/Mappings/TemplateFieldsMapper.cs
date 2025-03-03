using LiteDB;
using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Mappings;

internal class TemplateFieldsMapper
{
    [BsonField("name")]
    public string Name { get; init; } = string.Empty;

    [BsonField("value")]
    public string Value { get; init; } = string.Empty;

    [BsonField("label")]
    public string Label { get; init; } = string.Empty;

    public DetectedField ToDomain()
    {
        return new DetectedField(Value, Name, Label);
    }
}