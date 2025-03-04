namespace RuiSantos.AutoFill.Domain.Entities;

/// <summary>
/// Represents a detected field with its value, field name, and label.
/// </summary>
/// <param name="Value">The value of the detected field.</param>
/// <param name="FieldName">The name of the field.</param>
/// <param name="Label">The label associated with the field.</param>
public record TemplateField(
    string Value,
    string FieldName,
    string Label
);