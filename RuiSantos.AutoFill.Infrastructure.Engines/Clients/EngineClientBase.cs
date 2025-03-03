using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Clients;

/// <summary>
/// Base class for engine clients, providing common functionality for executing prompts and handling responses.
/// </summary>
/// <typeparam name="TSettings">The type of settings used by the engine client.</typeparam>
public abstract class EngineClientBase<TSettings> : IEngineClient
    where TSettings : EngineSettingsBase
{
    protected readonly TSettings settings;
    protected readonly IHttpClientFactory client;

    /// <summary>
    /// Initializes a new instance of the <see cref="EngineClientBase{TSettings}"/> class.
    /// </summary>
    /// <param name="options">The options containing the settings.</param>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    protected EngineClientBase(IOptions<TSettings> options, IHttpClientFactory httpClientFactory)
    {
        settings = options.Value;
        client = httpClientFactory.WithBaseUrl(settings.BaseUrl);
    }
    
    /// <summary>
    /// Executes a prompt asynchronously and returns the response as a string.
    /// </summary>
    /// <param name="prompt">The prompt to execute.</param>
    /// <returns>A task representing the asynchronous operation, with a string result.</returns>
    public Task<string?> ExecutePromptAsync(string prompt) => ExecutePromptAsync<string>(prompt);
    
    /// <summary>
    /// Executes a prompt asynchronously and returns the response as a specified type.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="prompt">The prompt to execute.</param>
    /// <returns>A task representing the asynchronous operation, with a response of type <typeparamref name="TResponse"/>.</returns>
    public abstract Task<TResponse?> ExecutePromptAsync<TResponse>(string prompt) where TResponse : class;

    /// <summary>
    /// Uploads a file and executes a prompt asynchronously.
    /// </summary>
    /// <param name="prompt">The prompt to execute.</param>
    /// <param name="fileName">The file to upload.</param>
    /// <returns>A task representing the asynchronous operation, with a string result.</returns>
    public abstract Task<string> UploadFileAndExecuteAsync(string prompt, string fileName);

    /// <summary>
    /// Determines whether the response contains errors.
    /// </summary>
    /// <param name="response">The HTTP post response.</param>
    /// <param name="message">The error message, if any.</param>
    /// <returns><c>true</c> if the response contains errors; otherwise, <c>false</c>.</returns>
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
    
    /// <summary>
    /// Gets the response as a specified type.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="responseText">The response text.</param>
    /// <returns>The response as a specified type, or <c>null</c> if the response is invalid.</returns>
    protected virtual TResponse? GetResponse<TResponse>(string? responseText) where TResponse : class
    {
        if (typeof(TResponse) == typeof(string))
            return responseText as TResponse;
        
        var json = GetJsonFromResponse(responseText)?.ToJsonString();
        return json is null 
            ? null 
            : JsonSerializer.Deserialize<TResponse>(json);
    }
    
    /// <summary>
    /// Extracts a JSON node from the response text.
    /// </summary>
    /// <param name="responseText">The response text.</param>
    /// <param name="rootElementName">The name of the root element, if any.</param>
    /// <returns>The extracted JSON node, or <c>null</c> if the response text is invalid.</returns>
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