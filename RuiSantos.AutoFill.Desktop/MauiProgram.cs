using LiteDB;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;
using RuiSantos.AutoFill.Application;
using RuiSantos.AutoFill.Desktop.ViewModels.Commons;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Clients;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb;

namespace RuiSantos.AutoFill.Desktop;

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
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            });

        builder.Services.AddMauiBlazorWebView();

        // Add MudBlazor services
        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            config.SnackbarConfiguration.ShowCloseIcon = true;
        });

        builder.Services
            .AddAllViewModels()
            .UseAutoFill()
            .UseLiteDb(options =>
            {
                options.Filename = Path.Combine(FileSystem.AppDataDirectory, "RuiSantos.AutoFill.db");
                options.Connection = ConnectionType.Shared;
            })
            .UseGeminiEngine(options =>
            {
                options.ApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? string.Empty;
            });

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}