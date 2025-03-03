using RuiSantos.AutoFill.Infrastructure.Engines.Clients;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Clients;

/// <summary>
/// Represents the settings for the Gemini engine.
/// </summary>
public class GeminiSettings: EngineSettingsBase
{
    /// <summary>
    /// Gets the API key required for authentication.
    /// </summary>
    public required string ApiKey { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiSettings"/> class.
    /// Sets the base URL and model name for the Gemini engine.
    /// </summary>
    public GeminiSettings(): base()
    {
        this.BaseUrl = "https://generativelanguage.googleapis.com/v1beta";
        this.ModelName = "gemini-2.0-flash:generateContent";
    }
}