using RestSharp;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Clients;

/// <summary>
/// Factory class for creating and configuring HTTP clients.
/// </summary>
public class HttpClientFactory: IHttpClientFactory
{
    private string _baseUrl = string.Empty;

    /// <summary>
    /// Sets the base URL for the HTTP client.
    /// </summary>
    /// <param name="baseUrl">The base URL to be used by the HTTP client.</param>
    /// <returns>The current instance of <see cref="HttpClientFactory"/>.</returns>
    public IHttpClientFactory WithBaseUrl(string baseUrl)
    {
        _baseUrl = baseUrl;
        return this;
    }

    /// <summary>
    /// Sends a POST request asynchronously.
    /// </summary>
    /// <param name="requestUrl">The URL for the request.</param>
    /// <param name="body">The body of the request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response.</returns>
    public async Task<HttpPostResponse> PostAsync(string requestUrl, object body)
    {
        using var client = new RestClient(_baseUrl);
        
        var request = new RestRequest(requestUrl, Method.Post);
        request.AddJsonBody(body, ContentType.Json);
        
        var response = await client.ExecuteAsync(request);
        
        return string.IsNullOrWhiteSpace(response.Content) 
            ? new HttpPostResponse(response.StatusCode, response.ErrorMessage ?? response.StatusDescription)
            : new HttpPostResponse(response.StatusCode, response.Content);
    }
}