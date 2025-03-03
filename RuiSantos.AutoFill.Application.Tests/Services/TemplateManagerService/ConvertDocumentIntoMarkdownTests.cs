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
}