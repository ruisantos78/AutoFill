using Microsoft.Extensions.Configuration;

namespace RuiSantos.AutoFill.App.Core.Extensions;

internal static class PreferencesConfigurationExtensions
{
    private static readonly IReadOnlyList<string> _settings =
    [
        "Engine:Gemini:ApiKey"
    ];
    
    public static IConfigurationBuilder AddPreferences(this IConfigurationBuilder builder)
    {
        return builder.Add(new PreferencesConfigurationSource());
    }

    private class PreferencesConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new PreferencesConfigurationProvider();
        }
    }

    private class PreferencesConfigurationProvider : ConfigurationProvider
    {
        public override void Load()
        {
            _settings.ToList().ForEach(Load);
        }

        private void Load(string key)
        {
            var value = Preferences.Get(key, string.Empty);
            if (!string.IsNullOrEmpty(key))
                Data[key] = value;
        }
    }
}