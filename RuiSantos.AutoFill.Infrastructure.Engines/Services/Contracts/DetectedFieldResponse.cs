using System.Text.Json.Serialization;
using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Services.Contracts;

/// <summary>
/// Represents the response of a detected field.
/// </summary>
internal class DetectedFieldResponse
{
    /// <summary>
    /// Gets the value of the detected field.
    /// </summary>
    [JsonPropertyName("value")]
    public required string Value { get; init; }

    /// <summary>
    /// Gets the name of the detected field.
    /// </summary>
    [JsonPropertyName("field_name")]
    public required string FieldName { get; init; }

    /// <summary>
    /// Gets the label of the detected field.
    /// </summary>
    [JsonPropertyName("label")]
    public required string Label { get; init; }

    /// <summary>
    /// Converts the detected field response to the domain entity <see cref="DetectedField"/>.
    /// </summary>
    /// <returns>An instance of <see cref="DetectedField"/>.</returns>
    public DetectedField ToDomain()
    {
        return new DetectedField(
            this.Value,
            this.FieldName,
            this.Label
        );
    }
}