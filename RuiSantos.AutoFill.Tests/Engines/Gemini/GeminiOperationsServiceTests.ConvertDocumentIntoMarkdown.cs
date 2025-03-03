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
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        var response = await Resources.GetStringAsync("Gemini.Contents.ConvertDocumentToMarkdown.Response");
        var expectedMarkdown = await Resources.GetStringAsync("Gemini.Contents.Default.Document");
        
        _client.PostAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(Task.FromResult(new HttpPostResponse(HttpStatusCode.OK, response)));
        
        // Act
        var markdown = await _service.ConvertDocumentToMarkdownAsync(filePath);
        
        // Assert
        Assert.NotNull(markdown);
        Assert.NotEmpty(markdown);
        Assert.Equal(expectedMarkdown, markdown);
    }

    [Fact(DisplayName = "ConvertDocumentIntoMarkdown with invalid file path should throw exception")]
    public async Task ConvertDocumentIntoMarkdown_InvalidFilePath_ShouldThrowException()
    {
        // Arrange
        var invalidFilePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "InvalidFile.ext");
        var expectedErrorMessage = $"Could not find file '{invalidFilePath}'.";
        
        // Act
        var exception = await Assert.ThrowsAsync<FileNotFoundException>(async () => 
            await _service.ConvertDocumentToMarkdownAsync(invalidFilePath));
        
        // Assert
        Assert.NotNull(exception);
        Assert.Equal(expectedErrorMessage, exception.Message);
    }
    
    [Fact(DisplayName = "ConvertDocumentIntoMarkdown with bad response should throw an exception")]
    public async Task ConvertDocumentIntoMarkdown_BadResponse_ThrowsException()
    {
        // Arrange
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        var error = await Resources.GetStringAsync("Gemini.Contents.Default.Error");
        
        _client.PostAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(Task.FromResult(new HttpPostResponse(HttpStatusCode.BadRequest, error)));
        
        // Act
        var exception = await Assert.ThrowsAsync<Exception>(async () => 
            await _service.ConvertDocumentToMarkdownAsync(filePath));
        
        // Assert
        Assert.Equal("API key not valid. Please pass a valid API key.", exception.Message);
    }
}