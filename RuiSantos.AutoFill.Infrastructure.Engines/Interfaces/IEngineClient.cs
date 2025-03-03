namespace RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

public interface IEngineClient
{
    Task<string?> ExecutePromptAsync(string prompt);
    Task<TResponse?> ExecutePromptAsync<TResponse>(string prompt) where TResponse : class;
}