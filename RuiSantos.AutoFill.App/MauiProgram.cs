using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using RuiSantos.AutoFill.App.Services;
using RuiSantos.AutoFill.App.UI.Pages;
using RuiSantos.AutoFill.App.ViewModels;
using RuiSantos.AutoFill.Application;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Clients;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb;

namespace RuiSantos.AutoFill.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit(options =>
            {
                options.SetShouldEnableSnackbarOnWindows(true);
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            });
        
        builder.Services
            .UseLiteDb(GetDatabaseFile)
            .UseGeminiEngine(GetEngineSettings)
            .UseAutoFill()
            .UseAutoFillApp();
        
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
        
        string GetDatabaseFile()
        {
            var databaseFile = Path.Combine(FileSystem.AppDataDirectory, "RuiSantos.AutoFill.db");
            return databaseFile;
        }
        
        async Task<GeminiSettings> GetEngineSettings()
        {
            var engineToken = await SecureStorage.Default.GetAsync("GeminiApiKey");
            return new GeminiSettings() { ApiKey = engineToken ?? "test_mode" };
        }
    }
    
    private static IServiceCollection UseAutoFillApp(this IServiceCollection services)
    {
        // Services
        services
            .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
            .AddSingleton<ITemplateServices, TemplateServices>();
        
        // ViewModels
        services
            .AddScoped<MainPage, MainViewModel>();
        
        return services;
    }
}