using NSubstitute;
using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Tests.Application;

partial class TemplateManagerServiceTests
{
    [Fact(DisplayName = "GenerateTemplateFromDocument with valid document should store and return the new template")]
    public async Task GenerateTemplateFromDocument_ValidDocument_ShouldStoreAndReturnTheNewTemplate()
    {
        // Arrange
        const string documentName = "TestDocument";
        const string documentText = "TestDocumentText";

        List<DetectedField> expectedFields =
        [
            new("Fulano de Tal", "nome_completo", "Nome Completo"),
            new("01/01/2000", "data_nascimento", "Data Nascimento")
        ];

        _engineOperations.DetectFieldsAndValuesAsync(documentText)
            .Returns(Task.FromResult(expectedFields));
        
        // Act
        var template = await _service.GenerateTemplateFromDocumentAsync(documentName, documentText);
        
        // Assert
        Assert.NotNull(template);
        Assert.NotEmpty(template.Fields);
        Assert.Equal(expectedFields, template.Fields);
        
        await _templateDocumentRepository.Received(1)
            .CreateAsync(Arg.Is<TemplateDocument>(x => x.Name == documentName));
    }

    [Fact(DisplayName = "GenerateTemplateFromDocument with invalid document should throw exception")]
    public async Task GenerateTemplateFromDocument_InvalidDocument_ShouldThrowException()
    {
        // Arrange
        const string documentName = "TestDocument";
        const string documentText = "TestDocumentText";
        
        _engineOperations.DetectFieldsAndValuesAsync(documentText)
            .Returns(Task.FromResult(new List<DetectedField>()));
        
        // Act
        var exception = await Assert.ThrowsAsync<TemplateManagerServiceException>(async () => 
            await _service.GenerateTemplateFromDocumentAsync(documentName, documentText));
        
        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.FieldAreNotDetectByEngine, exception.ErrorCode);
        Assert.Equal("GenerateTemplateFromDocumentAsync", exception.Action);
        Assert.Equal("The AI Engine has not detected any fields for this template", exception.Message);
        
        await _templateDocumentRepository.DidNotReceive()
            .CreateAsync(Arg.Is<TemplateDocument>(x => x.Name == documentName));
    }
}