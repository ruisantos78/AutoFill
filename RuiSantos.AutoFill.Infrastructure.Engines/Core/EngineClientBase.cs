using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Core;

public abstract class EngineClientBase<TSettings> : IEngineClient
    where TSettings : EngineSettingsBase
{
    protected readonly TSettings settings;
    protected readonly IHttpClientFactory client;

    protected EngineClientBase(IOptions<TSettings> options, IHttpClientFactory httpClientFactory)
    {
        settings = options.Value;
        client = httpClientFactory.WithBaseUrl(settings.BaseUrl);
    }
    
    public Task<string?> ExecutePromptAsync(string prompt) => ExecutePromptAsync<string>(prompt);
    
    public abstract Task<TResponse?> ExecutePromptAsync<TResponse>(string prompt) where TResponse : class;
    
    protected virtual bool HasErrors(HttpPostResponse response, out string message)
    {
        if (response.StatusCode is HttpStatusCode.OK)
        {
            message = string.Empty;
            return false;
        }

        message = response.Content ?? $"Unknown Error! Response Status: {response.StatusCode}";
        return true;
    }
    
    protected virtual TResponse? GetResponse<TResponse>(string? responseText) where TResponse : class
    {
        if (typeof(TResponse) == typeof(string))
            return responseText as TResponse;
        
        var json = GetJsonFromResponse(responseText)?.ToJsonString();
        return json is null 
            ? null 
            : JsonSerializer.Deserialize<TResponse>(json);
    }
    
    protected static JsonNode? GetJsonFromResponse(string? responseText, string rootElementName = "")
    {
        if (responseText is null) 
            return null;

        var firstIndex = responseText.IndexOf('[') < 0
            ? responseText.IndexOf('{')
            : Math.Min(responseText.IndexOf('['), responseText.IndexOf('{'));
        
        var lastIndex = responseText.IndexOf(']') < 0
            ? responseText.IndexOf('}') + 1
            : Math.Max(responseText.IndexOf(']'), responseText.IndexOf('}')) + 1;
        
        if (firstIndex < 0 || lastIndex <= firstIndex) 
            return null;
        
        var jsonText = responseText.Substring(firstIndex, lastIndex - firstIndex);
        if (string.IsNullOrEmpty(rootElementName))
            return JsonNode.Parse(jsonText);
        
        return JsonNode.Parse(jsonText)?[rootElementName];
    }
}