using Microsoft.Extensions.Configuration;

namespace RuiSantos.AutoFill.Application.Tests.Factories;

internal static class ConfigurationFactory
{
    public static IConfiguration Create()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true);

        return builder.Build();
    }
}