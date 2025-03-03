using NSubstitute;
using RuiSantos.AutoFill.Application.Services;
using RuiSantos.AutoFill.Domain.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;
using RuiSantos.AutoFill.Infrastructure.Repositories.Interfaces;

namespace RuiSantos.AutoFill.Tests.Application;

public partial class TemplateManagerServiceTests
{
    private readonly IEngineOperationsService _engineOperations = Substitute.For<IEngineOperationsService>();
    private readonly ITemplateDocumentRepository _templateDocumentRepository = Substitute.For<ITemplateDocumentRepository>();
    
    private readonly ITemplateManagerService _service;
    
    public TemplateManagerServiceTests()
    {
        _service = new TemplateManagerService(_engineOperations, _templateDocumentRepository);
    }
}