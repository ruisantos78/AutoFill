namespace RuiSantos.AutoFill.Infrastructure.Engines.Core;

public abstract class EngineSettingsBase
{
    public string ModelName { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = string.Empty;
    public decimal Temperature { get; init; }
}
