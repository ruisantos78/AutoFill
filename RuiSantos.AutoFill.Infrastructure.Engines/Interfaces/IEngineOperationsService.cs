using RuiSantos.AutoFill.Domain.Entities;

namespace RuiSantos.AutoFill.Infrastructure.Engines.Interfaces;

public interface IEngineOperationsService
{
    Task<List<DetectedField>> DetectFieldsAndValuesAsync(string documentText);
}