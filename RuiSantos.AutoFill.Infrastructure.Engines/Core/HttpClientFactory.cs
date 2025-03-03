using RestSharp;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Core;

public class HttpClientFactory: IHttpClientFactory
{
    private string _baseUrl = string.Empty;

    public IHttpClientFactory WithBaseUrl(string baseUrl)
    {
        _baseUrl = baseUrl;
        return this;
    }

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