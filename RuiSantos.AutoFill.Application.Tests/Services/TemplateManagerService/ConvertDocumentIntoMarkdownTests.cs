using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Application.Tests.Fixtures;
using RuiSantos.AutoFill.Domain.Interfaces;
using Xunit.Abstractions;

namespace RuiSantos.AutoFill.Application.Tests.Services.TemplateManagerService;

public class ConvertDocumentIntoMarkdownTests(ServiceProviderFixture provider, ITestOutputHelper output) 
    : IClassFixture<ServiceProviderFixture>
{
    private readonly ITemplateManagerService _service = provider.GetService<ITemplateManagerService>();
    
    [Fact(DisplayName = "ConvertDocumentIntoMarkdown with valid document should return a markdown file")]
    public async Task ConvertDocumentIntoMarkdown_ValidDocument_ShouldReturnAMarkdownFile()
    {
        // Arrange
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        
        // Act
        var markdown = await _service.ConvertDocumentIntoMarkdownAsync(filePath);
        
        // Assert
        Assert.NotNull(markdown);
        Assert.NotEmpty(markdown);
        
        output.WriteLine(markdown);
    }

    [Fact(DisplayName = "ConvertDocumentIntoMarkdown with invalid file path should throw exception")]
    public async Task ConvertDocumentIntoMarkdown_InvalidFilePath_ShouldThrowException()
    {
        // Arrange
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "InvalidFile.ext");
        var expectedMessage = $"The file {filePath} was not found.";
        
        // Act
        var exception = await Assert.ThrowsAsync<TemplateManagerServiceException>(async () => 
            await _service.ConvertDocumentIntoMarkdownAsync(filePath));
        
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
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Empty.txt");
        var expectedMessage = $"The conversion of the file {filePath} to Markdown failed.";
        
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