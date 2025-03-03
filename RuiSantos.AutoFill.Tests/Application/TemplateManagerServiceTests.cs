using NSubstitute;
using RuiSantos.AutoFill.Application.Services;
using RuiSantos.AutoFill.Domain.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;
using Xunit.Abstractions;

namespace RuiSantos.AutoFill.Tests.Application;

public partial class TemplateManagerServiceTests
{
    private readonly ITestOutputHelper _output;
    private readonly IEngineOperationsService _engineOperations = Substitute.For<IEngineOperationsService>();
    private readonly ITemplateDocumentRepository _templateDocumentRepository = Substitute.For<ITemplateDocumentRepository>();
    
    private readonly ITemplateManagerService _service;
    
    public TemplateManagerServiceTests(ITestOutputHelper output)
    {
        _output = output;
        _service = new TemplateManagerService(_engineOperations, _templateDocumentRepository);
    }
}