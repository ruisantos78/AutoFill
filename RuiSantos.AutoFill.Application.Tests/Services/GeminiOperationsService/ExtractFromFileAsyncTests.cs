using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Application.Tests.Fixtures;
using RuiSantos.AutoFill.Domain.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Mappings;
using Xunit.Abstractions;

namespace RuiSantos.AutoFill.Application.Tests.Services.GeminiOperationsService;

public class ExtractFromFileAsyncTests(ServiceProviderFixture provider, ITestOutputHelper output) 
    : IClassFixture<ServiceProviderFixture>
{
    private readonly IDataContext _dataContext = provider.GetService<IDataContext>();
    private readonly IGenerateTemplateService _service = provider.GetService<IGenerateTemplateService>();

    [Fact(DisplayName = "ExtractFromFileAsync with valid file path should return a template document")]
    public async Task ExtractFromFileAsync_ValidFilePath_ShouldReturnTemplateDocument()
    {
        // Arrange
        const string expectedTemplateName = "Sample";
        var executionTime = DateTime.Now;
        
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Sample.pdf");
        var fileName = Path.GetFileName(filePath);
        await using var fileStream = File.OpenRead(filePath);
        
        // Act
        var template = await _service.ExtractFromFileAsync(fileName, fileStream);
        
        // Assert
        Assert.NotNull(template);
        Assert.Equal(expectedTemplateName, template.Name);
        
        Assert.NotEmpty(template.Content);
        
        Assert.NotEmpty(template.Fields);
        Assert.Contains(template.Fields, f => f.Value == "João Carlos Silva Oliveira");
        Assert.Contains(template.Fields, f => f.Value == "Brazilian");
        Assert.Contains(template.Fields, f => f.Value == "single");
        Assert.Contains(template.Fields, f => f.Value == "12.345.678-9");
        Assert.Contains(template.Fields, f => f.Value == "123.456.789-10");
        
        var record = await _dataContext.FindAsync<TemplateDocumentMapper>(template.Id);
        Assert.NotNull(record);
        Assert.Equal(template.Name, record.Name);
        Assert.True(record.UpdatedAt >= executionTime);
        
        // Output
        output.WriteLine(template.Content);
    }
    
    [Fact(DisplayName = "ExtractFromFileAsync when conversion failed should throw exception")]
    public async Task ExtractFromFileAsync_WhenConversionFailed_ShouldThrowException()
    {
        // Arrange
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "Empty.txt");
        var fileName = Path.GetFileName(filePath);
        await using var fileStream = File.OpenRead(filePath);
        
        var documentName = Path.GetFileNameWithoutExtension(filePath);
        
        var expectedMessage = $"The conversion of the file {fileName} to Markdown failed.";
        
        // Act
        var exception = await Assert.ThrowsAsync<ApplicationServiceException>(async () => 
            await _service.ExtractFromFileAsync(fileName, fileStream));
        
        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.MarkdownConversionFailed, exception.ErrorCode);
        Assert.Equal("ConvertFileToMarkdownAsync", exception.Action);
        Assert.Equal(expectedMessage, exception.Message);
        
        var records = await _dataContext.FindAllAsync<TemplateDocumentMapper>(x => x.Name == documentName);
        Assert.Empty(records);
    }

    [Fact(DisplayName = "ExtractFromFileAsync when fields are not detected should throw exception")]
    public async Task ExtractFromFileAsync_WhenFieldsAreNotDetected_ShouldThrowException()
    {
        // Arrange
        var filePath = Path.Combine(Environment.CurrentDirectory, "Resources", "Files", "NonFields.pdf");
        var fileName = Path.GetFileName(filePath);
        await using var fileStream = File.OpenRead(filePath);
        
        var documentName = Path.GetFileNameWithoutExtension(filePath);
       
        // Act
        var exception = await Assert.ThrowsAsync<ApplicationServiceException>(async () => 
            await _service.ExtractFromFileAsync(fileName, fileStream));
        
        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.FieldAreNotDetectByEngine, exception.ErrorCode);
        Assert.Equal("DetectFieldsAsync", exception.Action);
        Assert.Equal("Fields are not detected by the engine.", exception.Message);

        var records = await _dataContext.FindAllAsync<TemplateDocumentMapper>(x => x.Name == documentName);
        Assert.Empty(records);
    }
}