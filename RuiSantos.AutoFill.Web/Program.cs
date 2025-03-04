using LiteDB;
using MudBlazor;
using MudBlazor.Services;
using RuiSantos.AutoFill.Application;
using RuiSantos.AutoFill.Infrastructure.Engines.Gemini;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb;
using RuiSantos.AutoFill.Web.Components;
using RuiSantos.AutoFill.Web.ViewModels.Commons;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    config.SnackbarConfiguration.ShowCloseIcon = true;
});

// Add AutoFill Services
builder.Services
    .AddAllViewModels()
    .UseAutoFill()
    .UseLiteDb(options =>
    {
        options.Filename = ":memory:";
        options.Connection = ConnectionType.Shared;
    })
    .UseGeminiEngine(options =>
    {
        options.ApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? string.Empty;
    });

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