using NSubstitute;
using RuiSantos.AutoFill.Application.Exceptions;

namespace RuiSantos.AutoFill.Tests.Application;

partial class TemplateManagerServiceTests
{
    [Fact(DisplayName = "ConvertDocumentIntoMarkdown with valid document should return a markdown file")]
    public async Task ConvertDocumentIntoMarkdown_ValidDocument_ShouldReturnAMarkdownFile()
    {
        // Arrange
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        const string expectedMarkdown = "This is a test";
        
        _engineOperations.ConvertDocumentToMarkdownAsync(filePath)
            .Returns(Task.FromResult(expectedMarkdown));
        
        // Act
        var markdown = await _service.ConvertDocumentIntoMarkdownAsync(filePath);
        
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
        var expectedMessage = $"The file {invalidFilePath} was not found.";
        
        // Act
        var exception = await Assert.ThrowsAsync<TemplateManagerServiceException>(async () => 
            await _service.ConvertDocumentIntoMarkdownAsync(invalidFilePath));
        
        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.FileNotFound, exception.ErrorCode);
        Assert.Equal("ConvertDocumentIntoMarkdownAsync", exception.Action);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact(DisplayName = "ConvertDocumentIntoMarkdown when conversion failed should throw exception")]
    public async Task ConvertDocumentIntoMarkdown_WhenConversionFailed_ShouldThrowException()
    {
        // Arrange
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        var expectedMessage = $"The conversion of the file {filePath} to Markdown failed.";
        
        _engineOperations.ConvertDocumentToMarkdownAsync(filePath)
            .Returns(Task.FromResult(string.Empty));
        
        // Act
        var exception = await Assert.ThrowsAsync<TemplateManagerServiceException>(async () => 
            await _service.ConvertDocumentIntoMarkdownAsync(filePath));
        
        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.MarkdownConversionFailed, exception.ErrorCode);
        Assert.Equal("ConvertDocumentIntoMarkdownAsync", exception.Action);
        Assert.Equal(expectedMessage, exception.Message);
    }
}