using Microsoft.AspNetCore.DataProtection;
using MudBlazor;
using MudBlazor.Services;
using RuiSantos.AutoFill.Application;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini;
using RuiSantos.AutoFill.Web.Components;
using RuiSantos.AutoFill.Web.ViewModels;
using RuiSantos.AutoFill.Web.ViewModels.Commons;

#if DEBUG
using LiteDB;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb;
#else
using MongoDB.Driver;
using RuiSantos.AutoFill.Infrastructure.Repositories.Mongo;
#endif

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices(configuration =>
{
    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
});

builder.Services.AddDataProtection()
    .SetApplicationName("AutoFill")
    .UseEphemeralDataProtectionProvider();

// Add AutoFill Services
builder.Services
    .AddAllServices()
    .UseAutoFill()
    .UseGeminiEngine(options =>
    {
        options.ApiKey = builder.Configuration.GetValue("GEMINI_API_KEY", string.Empty);
    });

#if DEBUG
builder.Services.UseLiteDb(options =>
{
    options.Filename = ":memory:";
    options.Connection = ConnectionType.Shared;
});
#else
builder.Services.UseMongoDb(options =>
{
    options.Server = new MongoServerAddress(
        host: builder.Configuration.GetValue("MONGO_SERVER_HOST", string.Empty),
        port: builder.Configuration.GetValue("MONGO_SERVER_PORT", 27017)
    );

    // Create credential using the admin database
    options.Credential = MongoCredential.CreateCredential(
        databaseName: "admin", // Always authenticate against admin
        username: builder.Configuration.GetValue("MONGO_USER", "admin"),
        password: builder.Configuration.GetValue("MONGO_PASSWORD", "securepassword").ToSecureString()
    );

    // Set database to use after authentication
    options.ApplicationName = builder.Configuration.GetValue("MONGO_DATABASE", "autofill");
});
#endif

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();