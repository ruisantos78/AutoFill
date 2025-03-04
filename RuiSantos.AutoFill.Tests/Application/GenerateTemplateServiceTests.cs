using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Application.Services;
using RuiSantos.AutoFill.Domain.Entities;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

namespace RuiSantos.AutoFill.Tests.Application;

public class GenerateTemplateServiceTests
{
    private readonly ILogger<GenerateTemplateService> _logger = Substitute.For<ILogger<GenerateTemplateService>>();
    private readonly IEngineOperationsService _engineOperations = Substitute.For<IEngineOperationsService>();
    private readonly ITemplateDocumentRepository _templateDocumentRepository = Substitute.For<ITemplateDocumentRepository>();

    private readonly GenerateTemplateService _service;

    public GenerateTemplateServiceTests()
    {
        _service = new GenerateTemplateService(_engineOperations, _templateDocumentRepository, _logger);
    }

    #region ExtractFromFileAsync Method Tests

    [Fact(DisplayName = "ExtractFromFileAsync with valid file path should return a template document")]
    public async Task ExtractFromFileAsync_ValidFilePath_ShouldReturnTemplateDocument()
    {
        // Arrange
        const string markdown = "This is a test";
        const string documentName = "Sample";
       
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        var fileName = Path.GetFileName(filePath);
        await using var fileStream = File.OpenRead(filePath);
        
        IReadOnlyList<TemplateField> fields =
        [
            new("Fulano de Tal", "nome_completo", "Nome Completo"),
            new("01/01/2000", "data_nascimento", "Data Nascimento")
        ];
        
        TemplateDocument templateDocument = new(Guid.Empty, documentName, markdown, fields.ToArray());

        _engineOperations.ConvertDocumentToMarkdownAsync(fileName, fileStream)
            .Returns(markdown);
        
        _engineOperations.DetectFieldsAndValuesAsync(markdown)
            .Returns(fields.ToList());
        
        // Act
        var result = await _service.ExtractFromFileAsync(fileName, fileStream);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(documentName, result.Name);
        Assert.Equal(fields, result.Fields);
        
        await _templateDocumentRepository.Received(1).CreateAsync(result);
    }
    
    [Fact(DisplayName = "ExtractFromFileAsync when conversion failed should throw exception")]
    public async Task ExtractFromFileAsync_WhenConversionFailed_ShouldThrowException()
    {
        // Arrange
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        var fileName = Path.GetFileName(filePath);
        await using var fileStream = File.OpenRead(filePath);
        
        var expectedMessage = $"The conversion of the file {fileName} to Markdown failed.";
        
        
        _engineOperations.ConvertDocumentToMarkdownAsync(fileName, fileStream)
            .Returns(string.Empty);
        
        // Act
        var exception = await Assert.ThrowsAsync<ApplicationServiceException>(async () => 
            await _service.ExtractFromFileAsync(fileName, fileStream));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.MarkdownConversionFailed, exception.ErrorCode);
        Assert.Equal("ConvertFileToMarkdownAsync", exception.Action);
        Assert.Equal(expectedMessage, exception.Message);
    }
    
    [Fact(DisplayName = "ExtractFromFileAsync when fields are not detected should throw exception")]
    public async Task ExtractFromFileAsync_WhenFieldsAreNotDetected_ShouldThrowException()
    {
        // Arrange
        const string expectedMessage = "Fields are not detected by the engine.";
        
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        var fileName = Path.GetFileName(filePath);
        await using var fileStream = File.OpenRead(filePath);
        
        _engineOperations.ConvertDocumentToMarkdownAsync(fileName, fileStream)
            .Returns("This is a test");
        
        _engineOperations.DetectFieldsAndValuesAsync(Arg.Any<string>())
            .Returns([]);
        
        // Act
        var exception = await Assert.ThrowsAsync<ApplicationServiceException>(async () => 
            await _service.ExtractFromFileAsync(fileName, fileStream));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.FieldAreNotDetectByEngine, exception.ErrorCode);
        Assert.Equal("DetectFieldsAsync", exception.Action);
        Assert.Equal(expectedMessage, exception.Message);
    }
    
    [Fact(DisplayName = "ExtractFromFileAsync when an unknown error occurs should throw exception")]
    public async Task ExtractFromFileAsync_WhenUnknownErrorOccurs_ShouldThrowException()
    {
        // Arrange
        const string expectedMessage = "An unexpected error occurs.";
        
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        var fileName = Path.GetFileName(filePath);
        await using var fileStream = File.OpenRead(filePath);
        
        _engineOperations.ConvertDocumentToMarkdownAsync(fileName, fileStream)
            .ThrowsAsync(new InvalidOperationException());
        
        // Act
        var exception = await Assert.ThrowsAsync<ApplicationServiceException>(async () => 
            await _service.ExtractFromFileAsync(fileName, fileStream));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.UnexpectedError, exception.ErrorCode);
        Assert.Equal("ExtractFromFileAsync", exception.Action);
        Assert.Equal(expectedMessage, exception.Message);
    }

    #endregion
}