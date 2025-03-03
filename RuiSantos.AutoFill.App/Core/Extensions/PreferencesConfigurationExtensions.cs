using Microsoft.Extensions.Configuration;

namespace RuiSantos.AutoFill.App.Core.Extensions;

/// <summary>
/// Provides extension methods for adding preferences configuration to the configuration builder.
/// </summary>
internal static class PreferencesConfigurationExtensions
{
    private static readonly IReadOnlyList<string> _settings =
    [
        "Engine:Gemini:ApiKey"
    ];

    /// <summary>
    /// Adds preferences configuration to the configuration builder.
    /// </summary>
    /// <param name="builder">The configuration builder.</param>
    /// <returns>The configuration builder with preferences configuration added.</returns>
    public static IConfigurationBuilder AddPreferences(this IConfigurationBuilder builder)
    {
        return builder.Add(new PreferencesConfigurationSource());
    }

    /// <summary>
    /// Represents a source of preferences configuration.
    /// </summary>
    private class PreferencesConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Builds the preferences configuration provider.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <returns>The preferences configuration provider.</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new PreferencesConfigurationProvider();
        }
    }

    /// <summary>
    /// Provides preferences configuration data.
    /// </summary>
    private class PreferencesConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Loads the preferences configuration data.
        /// </summary>
        public override void Load()
        {
            _settings.ToList().ForEach(Load);
        }

        /// <summary>
        /// Loads a specific preference setting.
        /// </summary>
        /// <param name="key">The key of the preference setting.</param>
        private void Load(string key)
        {
            var value = Preferences.Get(key, string.Empty);
            if (!string.IsNullOrEmpty(key))
                Data[key] = value;
        }
    }
}