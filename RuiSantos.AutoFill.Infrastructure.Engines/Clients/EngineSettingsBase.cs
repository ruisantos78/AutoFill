namespace RuiSantos.AutoFill.Infrastructure.Engines.Clients;

/// <summary>
/// Base class for engine settings.
/// </summary>
public abstract class EngineSettingsBase
{
    /// <summary>
    /// Gets the model name.
    /// </summary>
    public string ModelName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the base URL.
    /// </summary>
    public string BaseUrl { get; init; } = string.Empty;

    /// <summary>
    /// Gets the temperature setting.
    /// </summary>
    public decimal Temperature { get; init; }
}
