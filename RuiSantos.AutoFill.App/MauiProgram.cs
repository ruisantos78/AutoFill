using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RuiSantos.AutoFill.App.Core.Extensions;
using RuiSantos.AutoFill.Application;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb;

namespace RuiSantos.AutoFill.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true)
            .AddPreferences();

        builder.Services
            .UseAutoFill(builder.Configuration)
            .UseLiteDb(builder.Configuration)
            .UseGeminiEngine(builder.Configuration);
        
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}