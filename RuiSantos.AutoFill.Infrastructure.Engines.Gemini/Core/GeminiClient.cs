using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Infrastructure.Engines.Core;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Core;

public class GeminiClient(IOptions<GeminiSettings> options, IHttpClientFactory httpClientFactory) 
    : EngineClientBase<GeminiSettings>(options, httpClientFactory)
{
    public override async Task<TResponse?> ExecutePromptAsync<TResponse>(string prompt) where TResponse : class
    {
        var request = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = prompt
                        }
                    }
                }
            }
        };
        
        var response = await client.PostAsync($"/models/{settings.ModelName}?key={settings.ApiKey}", request);
        if (HasErrors(response, out var errorMessage))
            throw new Exception(errorMessage);

        return GetResponse<TResponse>(response.Content);
    }

    protected override bool HasErrors(HttpPostResponse response, out string message)
    {
        if (response.StatusCode is HttpStatusCode.OK)
        {
            message = string.Empty;
            return false;
        }

        if (JsonNode.Parse(response.Content ?? "") is not { } root)
            return base.HasErrors(response, out message);
     
        message = root["error"]?["message"]?.GetValue<string>() ?? $"Unknown Error! Response Status: {response.StatusCode}";
        return true;
    }

    protected override TResponse? GetResponse<TResponse>(string? responseText) where TResponse : class
    {
        if (responseText is null) 
            return null;
        
        if (JsonNode.Parse(responseText) is not {} root)
            return base.GetResponse<TResponse>(responseText);

        var response = root["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.GetValue<string>();
        return base.GetResponse<TResponse>(response);
    }
}