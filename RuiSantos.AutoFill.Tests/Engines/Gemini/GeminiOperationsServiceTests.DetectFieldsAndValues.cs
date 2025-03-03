using System.Net;
using NSubstitute;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Tests.Core;

namespace RuiSantos.AutoFill.Tests.Engines.Gemini;

partial class GeminiOperationsServiceTests
{
    [Fact(DisplayName = "DetectFieldsAndValuesAsync with valid response should return a list of fields")]
    public async Task DetectFieldsAndValuesAsync_ValidResponse_ReturnsDetectedFields()
    {
        // Arrange
        var document = await Resources.GetStringAsync("Gemini.Contents.DetectFieldsAndValues.Document");
        var response = await Resources.GetStringAsync("Gemini.Contents.DetectFieldsAndValues.Response");
        
        _client.PostAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(Task.FromResult(new HttpPostResponse(HttpStatusCode.OK, response)));
        
        // Act
        var fields = await _service.DetectFieldsAndValuesAsync(document);

        // Assert
        Assert.NotEmpty(fields);
        Assert.Contains(fields, f => f.Value == "João Carlos Silva Oliveira");
        Assert.Contains(fields, f => f.Value == "Brasileiro");
        Assert.Contains(fields, f => f.Value == "solteiro");
        Assert.Contains(fields, f => f.Value == "12.345.678-9");
        Assert.Contains(fields, f => f.Value == "123.456.789-10");
    }
    
    [Fact(DisplayName = "DetectFieldsAndValuesAsync with bad response should throw an exception")]
    public async Task DetectFieldsAndValuesAsync_BadResponse_ThrowsException()
    {
        // Arrange
        var document = await Resources.GetStringAsync("Gemini.Contents.DetectFieldsAndValues.Document");
        var error = await Resources.GetStringAsync("Gemini.Contents.DetectFieldsAndValues.Error");
        
        _client.PostAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(Task.FromResult(new HttpPostResponse(HttpStatusCode.BadRequest, error)));
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () => 
            await _service.DetectFieldsAndValuesAsync(document));
        
        Assert.Equal("API key not valid. Please pass a valid API key.", exception.Message);
    }
}