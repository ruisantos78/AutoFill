using System.Text.Json.Serialization;
using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Contracts;

internal class DetectedFieldResponse
{
    [JsonPropertyName("value")]
    public required string Value { get; init; }
        
    [JsonPropertyName("field_name")]
    public required string FieldName { get; init; }
        
    [JsonPropertyName("label")]
    public required string Label { get; init; }

    public DetectedField ToDomain()
    {
        return new DetectedField(
            this.Value,
            this.FieldName,
            this.Label
        );
    }
}