namespace RuiSantos.AutoFill.Domain.Entities;

public record DetectedField(
    string Value,
    string FieldName,
    string Label
);