using RuiSantos.AutoFill.Infrastructure.Engines.Core;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Core;

public class GeminiSettings: EngineSettingsBase
{
    public required string ApiKey { get; init; }

    public GeminiSettings(): base()
    {
        this.BaseUrl = "https://generativelanguage.googleapis.com/v1beta";
        this.ModelName = "gemini-2.0-flash:generateContent";
    }
}