using System.Net;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

public interface IHttpClientFactory
{
    IHttpClientFactory WithBaseUrl(string baseUrl);
    
    Task<HttpPostResponse> PostAsync(string requestUrl, object body);
}

public readonly record struct HttpPostResponse(HttpStatusCode StatusCode, string? Content);