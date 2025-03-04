using System.Net;
using NSubstitute;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Tests.Core;

namespace RuiSantos.AutoFill.Tests.Engines.Gemini;

partial class GeminiOperationsServiceTests
{
    [Fact(DisplayName = "ConvertDocumentIntoMarkdown with valid document should return a markdown file")]
    public async Task ConvertDocumentIntoMarkdown_ValidDocument_ShouldReturnAMarkdownFile()
    {
        // Arrange
        var response = await Resources.GetStringAsync("Gemini.Contents.ConvertDocumentToMarkdown.Response");
        var expectedMarkdown = await Resources.GetStringAsync("Gemini.Contents.Default.Document");
        
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        await using var fileStream = File.OpenRead(filePath);
        
        _client.PostAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(Task.FromResult(new HttpPostResponse(HttpStatusCode.OK, response)));
        
        // Act
        var markdown = await _service.ConvertDocumentToMarkdownAsync(filePath, fileStream);
        
        // Assert
        Assert.NotNull(markdown);
        Assert.NotEmpty(markdown);
        Assert.Equal(expectedMarkdown, markdown);
    }
    
    [Fact(DisplayName = "ConvertDocumentIntoMarkdown with bad response should throw an exception")]
    public async Task ConvertDocumentIntoMarkdown_BadResponse_ThrowsException()
    {
        // Arrange
        var error = await Resources.GetStringAsync("Gemini.Contents.Default.Error");
        
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        await using var fileStream = File.OpenRead(filePath);
        
        _client.PostAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(Task.FromResult(new HttpPostResponse(HttpStatusCode.BadRequest, error)));
        
        // Act
        var exception = await Assert.ThrowsAsync<Exception>(async () => 
            await _service.ConvertDocumentToMarkdownAsync(filePath, fileStream));
        
        // Assert
        Assert.Equal("API key not valid. Please pass a valid API key.", exception.Message);
    }
}