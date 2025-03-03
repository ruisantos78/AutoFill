using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Infrastructure.Engines.Clients;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Gemini.Clients;
/// <summary>
/// Represents a client for interacting with the Gemini API.
/// </summary>
/// <param name="options">The settings for the Gemini client.</param>
/// <param name="httpClientFactory">The factory to create HTTP clients.</param>
public class GeminiClient(IOptions<GeminiSettings> options, IHttpClientFactory httpClientFactory) 
    : EngineClientBase<GeminiSettings>(options, httpClientFactory)
{
    /// <summary>
    /// Executes a prompt asynchronously and returns the response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="prompt">The prompt to be executed.</param>
    /// <returns>The response of the prompt execution.</returns>
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

    /// <summary>
    /// Checks if the response contains errors.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="message">The error message if any.</param>
    /// <returns>True if there are errors, otherwise false.</returns>
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

    /// <summary>
    /// Parses the response text and returns the response object.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="responseText">The response text.</param>
    /// <returns>The parsed response object.</returns>
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