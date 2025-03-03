using System.Net;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

/// <summary>
/// Interface for creating and configuring HTTP clients.
/// </summary>
public interface IHttpClientFactory
{
    /// <summary>
    /// Sets the base URL for the HTTP client.
    /// </summary>
    /// <param name="baseUrl">The base URL to be used by the HTTP client.</param>
    /// <returns>The current instance of <see cref="IHttpClientFactory"/>.</returns>
    IHttpClientFactory WithBaseUrl(string baseUrl);
    
    /// <summary>
    /// Sends a POST request asynchronously.
    /// </summary>
    /// <param name="requestUrl">The URL to which the request is sent.</param>
    /// <param name="body">The body of the request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response.</returns>
    Task<HttpPostResponse> PostAsync(string requestUrl, object body);
}

/// <summary>
/// Represents the response from an HTTP POST request.
/// </summary>
/// <param name="StatusCode">The status code of the HTTP response.</param>
/// <param name="Content">The content of the HTTP response.</param>
public readonly record struct HttpPostResponse(HttpStatusCode StatusCode, string? Content);