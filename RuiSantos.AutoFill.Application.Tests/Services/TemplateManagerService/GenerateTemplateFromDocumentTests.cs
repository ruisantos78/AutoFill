using RuiSantos.AutoFill.Application.Exceptions;
using RuiSantos.AutoFill.Application.Tests.Factories;
using RuiSantos.AutoFill.Application.Tests.Fixtures;
using RuiSantos.AutoFill.Domain.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.LiteDb.Mappings;
using Xunit.Abstractions;

namespace RuiSantos.AutoFill.Application.Tests.Services.TemplateManagerService;

public class GenerateTemplateFromDocumentTests(ServiceProviderFixture provider) 
    : IClassFixture<ServiceProviderFixture>
{
    private IDataContext DataContext => provider.GetService<IDataContext>();
    private readonly ITemplateManagerService _service = provider.GetService<ITemplateManagerService>();
    
    [Fact(DisplayName = "GenerateTemplateFromDocument with valid document should store and return the new template")]
    public async Task GenerateTemplateFromDocument_ValidDocument_GeneratesTemplate()
    {
        // Arrange
        const string documentName = "AD ET EXTRA JUDICIA";
        var document = await DocumentFactory.Create(Documents.AdEtExtraJudicia);
        
        var executionTime = DateTime.Now;
        
        // Act
        var template = await _service.GenerateTemplateFromDocumentAsync(documentName, document);

        // Assert
        Assert.NotNull(template);

        Assert.NotEmpty(template.Fields);
        Assert.Contains(template.Fields, f => f.Value == "João Carlos Silva Oliveira");
        Assert.Contains(template.Fields, f => f.Value == "Brazilian");
        Assert.Contains(template.Fields, f => f.Value == "single");
        Assert.Contains(template.Fields, f => f.Value == "12.345.678-9");
        Assert.Contains(template.Fields, f => f.Value == "123.456.789-10");

        var record = await DataContext.FindAsync<TemplateDocumentMapper>(template.Id);
        Assert.NotNull(record);
        Assert.Equal(template.Name, record.Name);
        Assert.True(record.UpdatedAt >= executionTime);
    }
    
    [Fact(DisplayName = "GenerateTemplateFromDocument with invalid document should throw exception")]
    public async Task GenerateTemplateFromDocument_NoFieldsFound_ThrowsTemplateManagerServiceException()
    {
        // Arrange
        const string documentName = "EMPTY FILE";
        const string document = "This document doesn't have any fields.";
        
        // Act
        var exception = await Assert.ThrowsAsync<TemplateManagerServiceException>(async () => 
            await _service.GenerateTemplateFromDocumentAsync(documentName, document));
        
        // Assert
        Assert.NotNull(exception);
        Assert.Equal(ErrorCodes.FieldAreNotDetectByEngine, exception.ErrorCode);
        Assert.Equal("GenerateTemplateFromDocumentAsync", exception.Action);
        Assert.Equal("The AI Engine has not detected any fields for this template", exception.Message);

        var records = await DataContext
            .FindAllAsync<TemplateDocumentMapper>(x => x.Name == documentName);
        Assert.Empty(records);
    }
}